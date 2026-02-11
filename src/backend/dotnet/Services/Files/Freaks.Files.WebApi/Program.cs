using Freaks.Dal.Common.Extensions;
using Freaks.Files.Bll.Implementation;
using Freaks.Files.Dal.Implementation;
using Freaks.Telemetry.Common.Extensions;
using Freaks.Users.Common;
using Freaks.WebApi.Common.Extensions;
using Mapster;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDefaults(builder.Configuration);
builder.Services.AddControllers();
builder.Services.AddNSwag();
builder.Services.AddMapster();
builder.AddNLogCommon();
builder.Services.AddMetricsAndTraces(builder.Environment, builder.Configuration);

builder.Services.AddEasyCachingCommon(builder.Configuration);

builder.Services.AddKeycloakAuth(builder.Configuration);
builder.Services.AddUserContext(builder.Configuration);

builder.Services
       .AddBllServices()
       .AddDalProviders(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment()
    || app.Environment.IsCompose())
{
    app.UseNSwag();
}

app.UseDefaults();

app.UseAuthentication();
app.UseAuthorization();

app.UseUserContext();
app.MapControllers();

app.Run();