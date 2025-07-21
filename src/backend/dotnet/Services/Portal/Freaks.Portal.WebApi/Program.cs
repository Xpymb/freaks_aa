using Freaks.Dal.Common.Extensions;
using Freaks.Portal.Bll.Implementation;
using Freaks.Portal.Dal.Implementation;
using Freaks.Portal.Dal.Interfaces;
using Freaks.Users;
using Freaks.WebApi.Common.Extensions;
using Microsoft.EntityFrameworkCore;

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

    using var scope = app.Services.CreateScope();
    var dbContext = scope.ServiceProvider.GetRequiredService<IPortalDbContext>();
    await dbContext.Database.MigrateAsync();
}

app.UseAuthentication();
app.UseAuthorization();

app.UseUserContext();
app.UseCustomExceptionHandler();

app.MapControllers();

app.Run();