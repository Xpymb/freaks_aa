using Freaks.Portal.Bll.Interfaces.SalarySummary;
using Freaks.Portal.SharedContracts.Dto.SalarySummary;
using Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryExpenses;
using Freaks.Portal.SharedContracts.ValueObjects.SalarySummary;
using Freaks.Users.Common.Attributes;
using Freaks.Users.Contracts.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freaks.Portal.WebApi.Controllers.SalarySummary;

/// <summary>
///     Контроллер для управления расходами гильдии в зарплатных периодах.
///     Позволяет просматривать, добавлять, изменять и удалять расходы гильдии.
/// </summary>
[ApiController]
[Authorize]
[RequireRoles(UserRole.Member)]
[Route("salaries/{salaryId:long}/expenses")]
public class SalaryExpensesController : ControllerBase
{
    private readonly ISalaryExpensesService _service;

    /// <summary>
    ///     Инициализирует новый экземпляр контроллера для управления расходами гильдии в зарплатном периоде.
    /// </summary>
    /// <param name="service">Сервис для работы с расходами гильдии в зарплатном периоде.</param>
    public SalaryExpensesController(ISalaryExpensesService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    ///     Получает список расходов гильдии, связанных с указанным зарплатным периодом.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <returns>Список расходов гильдии в зарплатном периоде.</returns>
    [HttpGet]
    public Task<IList<SalaryExpensesDto>> GetListAsync([FromRoute] long salaryId)
    {
        return _service.GetListAsync(salaryId);
    }

    /// <summary>
    ///     Добавляет новую запись о расходе гильдии в указанный зарплатный период.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <param name="request">Данные нового расхода гильдии.</param>
    /// <returns>Информация о добавленном расходе гильдии.</returns>
    [RequireRoles(UserRole.Admin, UserRole.Editor, UserRole.GuildLeader)]
    [HttpPost]
    public Task<SalaryExpensesDto> CreateAsync([FromRoute] long salaryId, [FromBody] CreateSalaryExpensesRequest request)
    {
        return _service.CreateAsync(salaryId, request);
    }

    /// <summary>
    ///     Обновляет информацию о расходе гильдии в указанном зарплатном периоде.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <param name="id">Идентификатор статьи расхода.</param>
    /// <param name="request">Обновлённые данные о расходе гильдии.</param>
    /// <returns>Информация об обновлённом расходе гильдии.</returns>
    [RequireRoles(UserRole.Admin, UserRole.Editor, UserRole.GuildLeader)]
    [HttpPut("{id:long}")]
    public Task<SalaryExpensesDto> UpdateAsync(
        [FromRoute] long salaryId,
        [FromRoute] long id,
        [FromBody] UpdateSalaryExpensesRequest request)
    {
        return _service.UpdateAsync(id, salaryId, request);
    }

    /// <summary>
    ///     Удаляет запись о расходе гильдии из указанного зарплатного периода.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <param name="id">Идентификатор статьи расхода.</param>
    /// <returns>Результат выполнения операции.</returns>
    [RequireRoles(UserRole.Admin, UserRole.Editor, UserRole.GuildLeader)]
    [HttpDelete("{id:long}")]
    public Task DeleteAsync([FromRoute] long salaryId, [FromRoute] long id)
    {
        return _service.DeleteAsync(id, salaryId);
    }
}
