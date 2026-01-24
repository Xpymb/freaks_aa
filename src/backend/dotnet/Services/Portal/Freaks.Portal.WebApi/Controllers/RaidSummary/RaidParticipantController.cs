using Freaks.Portal.Bll.Interfaces.RaidSummary;
using Freaks.Portal.SharedContracts.Dto.RaidSummary;
using Freaks.Portal.SharedContracts.Requests.RaidSummary.RaidParticipant;
using Freaks.Users.Common.Attributes;
using Freaks.Users.Contracts.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freaks.Portal.WebApi.Controllers.RaidSummary;

/// <summary>
///     Контроллер для управления участниками рейда.
///     Позволяет просматривать список участников, добавлять новых и удалять существующих.
/// </summary>
[ApiController]
[Authorize]
[RequireRoles(UserRole.Member)]
[Route("raids/{raidId:long}/participants")]
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
    public Task<IList<RaidParticipantDto>> GetListAsync([FromRoute] long raidId)
    {
        return _service.GetListAsync(raidId);
    }

    /// <summary>
    ///     Добавляет нового участника в рейд.
    /// </summary>
    /// <param name="raidId">Идентификатор рейда.</param>
    /// <param name="request">Данные нового участника.</param>
    /// <returns>Информация о добавленном участнике.</returns>
    [HttpPost]
    public Task<RaidParticipantDto> CreateAsync([FromRoute] long raidId, [FromBody] CreateRaidParticipantRequest request)
    {
        return _service.CreateAsync(raidId, request);
    }

    /// <summary>
    ///     Удаляет участника из рейда.
    /// </summary>
    /// <param name="raidId">Идентификатор рейда.</param>
    /// <param name="participantId">Идентификатор участника.</param>
    /// <returns>Результат выполнения операции.</returns>
    [HttpDelete("{participantId:Guid}")]
    public Task DeleteAsync([FromRoute] long raidId, [FromRoute] Guid participantId)
    {
        return _service.DeleteAsync(raidId, participantId);
    }
}