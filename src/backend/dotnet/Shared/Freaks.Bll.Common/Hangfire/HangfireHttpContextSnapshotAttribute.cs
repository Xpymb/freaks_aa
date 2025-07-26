using System.Security.Claims;
using System.Text.Json;
using Freaks.Users.Contracts.ValueObjects;
using Hangfire.Client;
using Hangfire.Common;
using Hangfire.Server;
using Microsoft.AspNetCore.Http;

namespace Freaks.Bll.Common.Hangfire;

public class HangfireHttpContextSnapshotAttribute :
    JobFilterAttribute,
    IClientFilter,
    IServerFilter
{
    private const string SnapshotKey = "HttpContextSnapshot";

    private readonly IHttpContextAccessor _httpAccessor;

    public HangfireHttpContextSnapshotAttribute(IHttpContextAccessor httpAccessor)
    {
        _httpAccessor = httpAccessor ?? throw new ArgumentNullException(nameof(httpAccessor));
    }

    /// <inheritdoc />
    public void OnCreating(CreatingContext context)
    {
        var ctx = _httpAccessor.HttpContext;
        if (ctx == null)
        {
            return;
        }

        var user = _httpAccessor.HttpContext?.User;
        if (user is null)
        {
            return;
        }

        if (!ctx.Items.TryGetValue("UserContext", out var userContextItem)
            || userContextItem is not IUserContext userContext)
        {
            return;
        }

        var httpContextSnapshot =
            new HangfireHttpContextSnapshot
            {
                Claims =
                    user.Claims
                        .Select(c =>
                                    new HangfireClaimSnapshot
                                    {
                                        Type = c.Type, Value = c.Value,
                                    })
                        .ToList(),
                UserContext = userContext,
            };

        var json = JsonSerializer.Serialize(httpContextSnapshot);
        context.SetJobParameter(SnapshotKey, json);
    }

    /// <inheritdoc />
    public void OnCreated(CreatedContext context)
    {
    }

    /// <inheritdoc />
    public void OnPerforming(PerformingContext context)
    {
        var snapshotJson = context.GetJobParameter<string>(SnapshotKey);
        if (string.IsNullOrEmpty(snapshotJson))
        {
            return;
        }

        var snapshot = JsonSerializer.Deserialize<HangfireHttpContextSnapshot>(snapshotJson);
        if (snapshot is null)
        {
            return;
        }

        var fakeHttpContext = new DefaultHttpContext();

        var identity =
            new ClaimsIdentity(
                snapshot.Claims.Select(sc => new Claim(sc.Type, sc.Value)),
                "Bearer");

        fakeHttpContext.User = new ClaimsPrincipal(identity);
        fakeHttpContext.Items.TryAdd("UserContext", snapshot.UserContext);

        _httpAccessor.HttpContext = fakeHttpContext;
    }

    /// <inheritdoc />
    public void OnPerformed(PerformedContext context)
    {
    }
}