using Freaks.Bll.Common.Extensions;
using Freaks.Dal.Common.Extensions;
using Freaks.Messages.Common;
using Freaks.Portal.Bll.Implementation;
using Freaks.Portal.Dal.Implementation;
using Freaks.Portal.Dal.Persistence;
using Freaks.Users.Common;
using Freaks.Users.Dal.Persistence;
using Freaks.WebApi.Common.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Common
builder.Services.AddDefaults(builder.Configuration);
builder.Services.AddNSwag();
builder.Services.AddMapsterCommon();
builder.Services.AddCentrifugoMessageService(builder.Configuration);

// Auth
builder.Services.AddKeycloakAuth(builder.Configuration);

// Cache
builder.Services.AddEasyCachingCommon(builder.Configuration);

// User
builder.Services.AddUserContext(builder.Configuration);

// Core
builder.Services
       .AddBllServices(builder.Configuration)
       .AddDalProviders(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment()
    || app.Environment.IsCompose())
{
    app.UseNSwag();

    app.UseCors(options =>
                    options
                        .AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader()
                        .WithExposedHeaders("Content-Disposition"));
}

await app.ApplyMigrationsAsync<IUserDbContext>();
await app.ApplyMigrationsAsync<IPortalDbContext>();

app.UseDefaults();

app.UseAuthentication();
app.UseAuthorization();

app.UseUserContext();
app.UseCustomExceptionHandler();

app.MapControllers();

app.Run();