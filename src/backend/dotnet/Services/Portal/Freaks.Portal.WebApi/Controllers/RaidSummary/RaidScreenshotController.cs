using Freaks.Portal.Bll.Interfaces.RaidSummary;
using Freaks.Portal.SharedContracts.Dto.RaidSummary;
using Freaks.Portal.SharedContracts.Requests.RaidSummary.RaidScreenshot;
using Freaks.Users.Common.Attributes;
using Freaks.Users.Contracts.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freaks.Portal.WebApi.Controllers.RaidSummary;

/// <summary>
/// Контроллер для управления скриншотами, загруженными после рейда.
/// Позволяет просматривать, сохранять и удалять скриншоты.
/// </summary>
[ApiController]
[Authorize]
[RequireRoles(UserRole.Member)]
[Route("raids/{raidId:long}/screenshots")]
public class RaidScreenshotController : ControllerBase
{
    private readonly IRaidScreenshotService _service;

    /// <summary>
    /// Инициализирует новый экземпляр контроллера для работы со скриншотами рейда.
    /// </summary>
    /// <param name="service">Сервис для работы со скриншотами рейда.</param>
    public RaidScreenshotController(IRaidScreenshotService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    /// Получает список скриншотов, связанных с указанным рейдом.
    /// </summary>
    /// <param name="raidId">Идентификатор рейда.</param>
    /// <returns>Список скриншотов.</returns>
    [HttpGet]
    public async Task<IList<RaidScreenshotDto>> GetListAsync([FromRoute] long raidId)
    {
        return await _service.GetListAsync(raidId);
    }

    /// <summary>
    /// Устанавливает (добавляет или заменяет) список скриншотов для указанного рейда.
    /// </summary>
    /// <param name="raidId">Идентификатор рейда.</param>
    /// <param name="request">Данные скриншотов для установки.</param>
    /// <returns>Актуальный список скриншотов после обновления.</returns>
    [HttpPost]
    public async Task<IList<RaidScreenshotDto>> SetAsync([FromRoute] long raidId, [FromBody] SetRaidScreenshotRequest request)
    {
        return await _service.SetAsync(raidId, request);
    }

    /// <summary>
    /// Удаляет указанный скриншот из рейда.
    /// </summary>
    /// <param name="raidId">Идентификатор рейда.</param>
    /// <param name="screenshotUrl">URL скриншота для удаления.</param>
    /// <returns>Результат выполнения операции.</returns>
    [HttpDelete]
    public async Task<ActionResult> DeleteAsync([FromRoute] long raidId, [FromQuery] string screenshotUrl)
    {
        await _service.DeleteAsync(raidId, screenshotUrl);
        return Ok();
    }
}
