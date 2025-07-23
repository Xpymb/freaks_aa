using Freaks.Files.Bll.Implementation;
using Freaks.Files.Dal.Implementation;
using Freaks.WebApi.Common.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddNSwag();

builder.Services.AddKeycloakAuth(builder.Configuration);

builder.Services
       .AddBllServices()
       .AddDalProviders(builder.Configuration);

var app = builder.Build();

if (app.Environment.IsDevelopment()
    || app.Environment.IsCompose())
{
    app.UseNSwag();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();