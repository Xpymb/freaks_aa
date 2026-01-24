using Freaks.Portal.Bll.Interfaces.RaidSummary;
using Freaks.Portal.SharedContracts.Dto.RaidSummary;
using Freaks.Portal.SharedContracts.Requests.RaidSummary.Raid;
using Freaks.SharedContracts.Common;
using Freaks.Users.Common.Attributes;
using Freaks.Users.Contracts.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freaks.Portal.WebApi.Controllers.RaidSummary;

/// <summary>
///     Контроллер для управления рейдами.
///     Предоставляет API для получения, создания, обновления и удаления рейдов.
/// </summary>
[ApiController]
[Authorize]
[RequireRoles(UserRole.Member)]
[Route("raids")]
public class RaidController : ControllerBase
{
    private readonly IRaidService _service;

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="RaidController" />.
    /// </summary>
    /// <param name="service">Сервис, реализующий бизнес-логику работы с рейдами.</param>
    public RaidController(IRaidService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    ///     Возвращает рейд по указанному идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор рейда.</param>
    /// <returns>Объект <see cref="RaidDto" /> с полной информацией о рейде.</returns>
    [HttpGet("{id:long}")]
    public Task<RaidDto> GetAsync([FromRoute] long id)
    {
        return _service.GetAsync(id);
    }

    /// <summary>
    ///     Возвращает постраничный список рейдов по фильтру и сортировке.
    /// </summary>
    /// <param name="request">Параметры фильтрации, сортировки и пагинации.</param>
    /// <returns>Список рейдов в виде кратких DTO.</returns>
    [HttpGet]
    public Task<PaginatedList<RaidShortDto>> GetListAsync([FromQuery] GetRaidListRequest request)
    {
        return _service.GetListAsync(request);
    }

    /// <summary>
    ///     Создаёт новый рейд.
    /// </summary>
    /// <param name="request">Данные для создания рейда.</param>
    /// <returns>Созданный рейд в виде <see cref="RaidDto" />.</returns>
    [HttpPost]
    public Task<RaidDto> CreateAsync([FromBody] CreateRaidRequest request)
    {
        return _service.CreateAsync(request);
    }

    /// <summary>
    ///     Завершает существующий рейд.
    /// </summary>
    /// <param name="id">Идентификатор рейда.</param>
    /// <returns>Завершенный рейд в виде <see cref="RaidDto" />.</returns>
    [RequireRoles(UserRole.Admin, UserRole.GuildLeader)]
    [HttpPost("{id:long}/finish")]
    public Task<RaidDto> FinishAsync([FromRoute] long id)
    {
        return _service.FinishAsync(id);
    }

    /// <summary>
    ///     Обновляет существующий рейд.
    /// </summary>
    /// <param name="id">Идентификатор рейда, который нужно обновить.</param>
    /// <param name="request">Обновлённые данные рейда.</param>
    /// <returns>Обновлённый рейд в виде <see cref="RaidDto" />.</returns>
    [HttpPut("{id:long}")]
    public Task<RaidDto> UpdateAsync([FromRoute] long id, [FromBody] UpdateRaidRequest request)
    {
        return _service.UpdateAsync(id, request);
    }

    /// <summary>
    ///     Удаляет рейд по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор рейда, подлежащего удалению.</param>
    /// <returns>Результат выполнения операции.</returns>
    [HttpDelete("{id:long}")]
    public Task DeleteAsync([FromRoute] long id)
    {
        return _service.DeleteAsync(id);
    }
}