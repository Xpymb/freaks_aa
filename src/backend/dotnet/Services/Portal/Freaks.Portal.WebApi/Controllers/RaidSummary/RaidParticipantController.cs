using Freaks.Portal.Bll.Interfaces.RaidSummary;
using Freaks.Portal.SharedContracts.Dto.RaidSummary;
using Freaks.Portal.SharedContracts.Requests.RaidSummary.RaidParticipant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freaks.Portal.WebApi.Controllers.RaidSummary;

/// <summary>
///     Контроллер для управления участниками рейда.
///     Позволяет просматривать список участников, добавлять новых и удалять существующих.
/// </summary>
[Authorize]
[ApiController]
[Route("raids/{raidId:int}/participants")]
public class RaidParticipantController : ControllerBase
{
    private readonly IRaidParticipantService _service;

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="RaidParticipantController" />.
    /// </summary>
    /// <param name="service">Сервис для работы с участниками рейда.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если <paramref name="service" /> равен null.</exception>
    public RaidParticipantController(IRaidParticipantService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    ///     Получает список участников для указанного рейда.
    /// </summary>
    /// <param name="raidId">Идентификатор рейда.</param>
    /// <returns>Список участников рейда.</returns>
    [HttpGet]
    public async Task<IList<RaidParticipantDto>> GetListAsync([FromRoute] int raidId)
    {
        return await _service.GetListAsync(raidId);
    }

    /// <summary>
    ///     Добавляет нового участника в рейд.
    /// </summary>
    /// <param name="raidId">Идентификатор рейда.</param>
    /// <param name="request">Данные нового участника.</param>
    /// <returns>Информация о добавленном участнике.</returns>
    [HttpPost]
    public async Task<RaidParticipantDto> CreateAsync([FromRoute] int raidId, [FromBody] CreateRaidParticipantRequest request)
    {
        var result = await _service.CreateAsync(raidId, request);
        return result;
    }

    /// <summary>
    ///     Удаляет участника из рейда.
    /// </summary>
    /// <param name="raidId">Идентификатор рейда.</param>
    /// <param name="participantId">Идентификатор участника.</param>
    /// <returns>Результат выполнения операции.</returns>
    [HttpDelete("{participantId:Guid}")]
    public async Task<ActionResult> DeleteAsync([FromRoute] int raidId, [FromRoute] Guid participantId)
    {
        await _service.DeleteAsync(raidId, participantId);
        return Ok();
    }
}