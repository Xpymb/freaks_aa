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
    [HttpGet("{id:int}")]
    public async Task<RaidDto> GetAsync([FromRoute] int id)
    {
        return await _service.GetAsync(id);
    }

    /// <summary>
    ///     Возвращает постраничный список рейдов по фильтру и сортировке.
    /// </summary>
    /// <param name="request">Параметры фильтрации, сортировки и пагинации.</param>
    /// <returns>Список рейдов в виде кратких DTO.</returns>
    [HttpGet]
    public async Task<PaginatedList<RaidShortDto>> GetListAsync([FromQuery] GetRaidListRequest request)
    {
        return await _service.GetListAsync(request);
    }

    /// <summary>
    ///     Создаёт новый рейд.
    /// </summary>
    /// <param name="request">Данные для создания рейда.</param>
    /// <returns>Созданный рейд в виде <see cref="RaidDto" />.</returns>
    [HttpPost]
    public async Task<RaidDto> CreateAsync([FromBody] CreateRaidRequest request)
    {
        return await _service.CreateAsync(request);
    }

    /// <summary>
    ///     Обновляет существующий рейд.
    /// </summary>
    /// <param name="id">Идентификатор рейда, который нужно обновить.</param>
    /// <param name="request">Обновлённые данные рейда.</param>
    /// <returns>Обновлённый рейд в виде <see cref="RaidDto" />.</returns>
    [HttpPut("{id:int}")]
    public async Task<RaidDto> UpdateAsync([FromRoute] int id, [FromBody] UpdateRaidRequest request)
    {
        return await _service.UpdateAsync(id, request);
    }

    /// <summary>
    ///     Удаляет рейд по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор рейда, подлежащего удалению.</param>
    /// <returns>Результат выполнения операции.</returns>
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteAsync([FromRoute] int id)
    {
        await _service.DeleteAsync(id);
        return Ok();
    }
}