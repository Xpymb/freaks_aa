using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freaks.Portal.WebApi.Controllers;

/// <summary>
///     Контроллер версий приложения
/// </summary>
[Authorize]
[ApiController]
[Route("app-version")]
public class AppVersionController : ControllerBase
{
    /// <summary>
    ///     Получить версию приложения
    /// </summary>
    /// <returns>Версия приложения</returns>
    [HttpGet]
    public string Get()
    {
        return "0.0.1";
    }
}