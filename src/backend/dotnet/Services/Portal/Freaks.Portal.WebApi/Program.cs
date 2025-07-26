using Freaks.Dal.Common.Extensions;
using Freaks.Portal.Bll.Implementation;
using Freaks.Portal.Dal.Implementation;
using Freaks.Portal.Dal.Persistence;
using Freaks.Users.Common;
using Freaks.Users.Dal.Persistence;
using Freaks.WebApi.Common.Extensions;
using Mapster;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Common
builder.Services.AddDefaults(builder.Configuration);
builder.Services.AddNSwag();
builder.Services.AddMapster();

// Auth
builder.Services.AddKeycloakAuth(builder.Configuration);

// Cache
builder.Services.AddEasyCaching(builder.Configuration);

// User
builder.Services.AddUserContext(builder.Configuration);
builder.Services.AddKeycloakAdmin(builder.Configuration);

// Core
builder.Services
       .AddBllServices(builder.Configuration)
       .AddDalProviders(builder.Configuration);

var app = builder.Build();

app.UsePathBase("/portal");

app.UseDefaults();

if (app.Environment.IsDevelopment()
    || app.Environment.IsCompose())
{
    app.UseNSwag();

    await app.ApplyMigrationsAsync<IUserDbContext>();
    await app.ApplyMigrationsAsync<IPortalDbContext>();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseUserContext();
app.UseCustomExceptionHandler();

app.MapControllers();

app.Run();