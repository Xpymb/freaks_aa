using Freaks.Portal.Bll.Interfaces.SalarySummary;
using Freaks.Portal.SharedContracts.Dto.SalarySummary;
using Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryGuildLeader;
using Freaks.Users.Common.Attributes;
using Freaks.Users.Contracts.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freaks.Portal.WebApi.Controllers.SalarySummary;

/// <summary>
///     Контроллер для управления долями руководства гильдии в зарплатных периодах.
///     Позволяет просматривать, добавлять, изменять и удалять доли руководства гильдии.
/// </summary>
[ApiController]
[Authorize]
[RequireRoles(UserRole.Member)]
[Route("salaries/{salaryId:long}/guild-leaders")]
public class SalaryGuildLeaderController : ControllerBase
{
    private readonly ISalaryGuildLeaderService _service;

    /// <summary>
    ///     Инициализирует новый экземпляр контроллера для управления долями руководства гильдии в зарплатном периоде.
    /// </summary>
    /// <param name="service">Сервис для работы с долями руководства гильдии в зарплатном периоде.</param>
    public SalaryGuildLeaderController(ISalaryGuildLeaderService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    ///     Получает список долей руководства гильдии, связанных с указанным зарплатным периодом.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <returns>Список долей руководства гильдии в зарплатном периоде.</returns>
    [HttpGet]
    public async Task<IList<SalaryGuildLeaderDto>> GetListAsync([FromRoute] long salaryId)
    {
        return await _service.GetListAsync(salaryId);
    }

    /// <summary>
    ///     Добавляет новую запись о доле руководства гильдии в указанный зарплатный период.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <param name="request">Данные новой доли руководства гильдии.</param>
    /// <returns>Информация о добавленной доле руководства гильдии.</returns>
    [RequireRoles(UserRole.Admin, UserRole.GuildLeader)]
    [HttpPost]
    public async Task<SalaryGuildLeaderDto> CreateAsync([FromRoute] long salaryId, [FromBody] CreateSalaryGuildLeaderRequest request)
    {
        return await _service.CreateAsync(salaryId, request);
    }

    /// <summary>
    ///     Обновляет информацию о доле руководства гильдии в указанном зарплатном периоде.
    ///     Amount пересчитывается по формуле: Quantity * PricePerLoot.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <param name="guildLeaderId">Идентификатор записи доли руководства гильдии.</param>
    /// <param name="request">Обновлённые данные о доле руководства гильдии.</param>
    /// <returns>Информация об обновлённой доле руководства гильдии.</returns>
    [RequireRoles(UserRole.Admin, UserRole.GuildLeader)]
    [HttpPut("{guildLeaderId:long}")]
    public async Task<SalaryGuildLeaderDto> UpdateAsync([FromRoute] long salaryId, [FromRoute] long guildLeaderId, [FromBody] UpdateSalaryGuildLeaderRequest request)
    {
        return await _service.UpdateAsync(salaryId, guildLeaderId, request);
    }

    /// <summary>
    ///     Удаляет запись о доле руководства гильдии из указанного зарплатного периода.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <param name="guildLeaderId">Идентификатор записи доли руководства гильдии.</param>
    /// <returns>Результат выполнения операции.</returns>
    [RequireRoles(UserRole.Admin, UserRole.GuildLeader)]
    [HttpDelete("{guildLeaderId:long}")]
    public async Task<ActionResult> DeleteAsync([FromRoute] long salaryId, [FromRoute] long guildLeaderId)
    {
        await _service.DeleteAsync(salaryId, guildLeaderId);
        return Ok();
    }
}
