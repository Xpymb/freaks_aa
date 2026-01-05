using Freaks.Portal.Bll.Interfaces.SalarySummary;
using Freaks.Portal.SharedContracts.Dto.SalarySummary;
using Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryLoot;
using Freaks.Users.Common.Attributes;
using Freaks.Users.Contracts.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freaks.Portal.WebApi.Controllers.SalarySummary;

/// <summary>
///     Контроллер для управления проданным лутом за зарплатные периоды.
///     Позволяет просматривать, добавлять, изменять и удалять лут.
/// </summary>
[ApiController]
[Authorize]
[RequireRoles(UserRole.Member)]
[Route("salaries/{salaryId:long}/loots")]
public class SalaryLootController : ControllerBase
{
    private readonly ISalaryLootService _service;

    /// <summary>
    ///     Инициализирует новый экземпляр контроллера для управления проданным лутом зарплатного периода.
    /// </summary>
    /// <param name="service">Сервис для работы с проданным лутом зарплатного периода.</param>
    public SalaryLootController(ISalaryLootService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    ///     Получает список проданного лута, связанного с указанным зарплатным периодом.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <returns>Список проданного лута зарплатного периода.</returns>
    [HttpGet]
    public async Task<IList<SalaryLootDto>> GetListAsync([FromRoute] long salaryId)
    {
        return await _service.GetListAsync(salaryId);
    }

    /// <summary>
    ///     Добавляет новую запись о проданном луте в указанный зарплатный период.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <param name="request">Данные нового проданного лута.</param>
    /// <returns>Информация о добавленном проданном луте.</returns>
    [RequireRoles(UserRole.Editor, UserRole.Admin, UserRole.GuildLeader)]
    [HttpPost]
    public async Task<SalaryLootDto> CreateAsync([FromRoute] long salaryId, [FromBody] CreateSalaryLootRequest request)
    {
        return await _service.CreateAsync(salaryId, request);
    }

    /// <summary>
    ///     Обновляет информацию о проданном луте в указанном зарплатном периоде.
    ///     Поддерживает обратный пересчёт финансовых полей.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <param name="lootId">Идентификатор записи проданного лута.</param>
    /// <param name="request">Обновлённые данные о проданном луте.</param>
    /// <returns>Информация об обновлённом проданном луте.</returns>
    [RequireRoles(UserRole.Editor, UserRole.Admin, UserRole.GuildLeader)]
    [HttpPut("{lootId:long}")]
    public async Task<SalaryLootDto> UpdateAsync([FromRoute] long salaryId, [FromRoute] long lootId, [FromBody] UpdateSalaryLootRequest request)
    {
        return await _service.UpdateAsync(salaryId, lootId, request);
    }

    /// <summary>
    ///     Удаляет запись о проданном луте из указанного зарплатного периода.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <param name="lootId">Идентификатор записи проданного лута.</param>
    /// <returns>Результат выполнения операции.</returns>
    [RequireRoles(UserRole.Editor, UserRole.Admin, UserRole.GuildLeader)]
    [HttpDelete("{lootId:long}")]
    public async Task<ActionResult> DeleteAsync([FromRoute] long salaryId, [FromRoute] long lootId)
    {
        await _service.DeleteAsync(salaryId, lootId);
        return Ok();
    }
}
