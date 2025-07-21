using Freaks.Dal.Common.Extensions;
using Freaks.Portal.Bll.Implementation;
using Freaks.Portal.Dal.Implementation;
using Freaks.Portal.Dal.Persistence;
using Freaks.Users;
using Freaks.Users.Persistence;
using Freaks.WebApi.Common.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDefaults(builder.Configuration);

// Swagger
builder.Services.AddNSwag();

// Auth
builder.Services.AddKeycloakAuth(builder.Configuration);

// Cache
builder.Services.AddEasyCaching(builder.Configuration);

// User
builder.Services.AddUserContext(builder.Configuration);

// Core
builder.Services
       .AddBllServices()
       .AddDalProviders(builder.Configuration);

var app = builder.Build();

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