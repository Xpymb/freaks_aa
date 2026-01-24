using Freaks.Portal.Bll.Interfaces.SalarySummary;
using Freaks.Portal.SharedContracts.Dto.SalarySummary;
using Freaks.Portal.SharedContracts.Requests.SalarySummary.Salary;
using Freaks.SharedContracts.Common;
using Freaks.Users.Common.Attributes;
using Freaks.Users.Contracts.ValueObjects;
using Freaks.WebApi.Common.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freaks.Portal.WebApi.Controllers.SalarySummary;

/// <summary>
///     Контроллер для управления зарплатными периодами.
/// </summary>
[ApiController]
[Authorize]
[RequireRoles(UserRole.Member)]
[Route("salaries")]
public class SalaryController : ControllerBase
{
    private readonly ISalaryService _service;
    private readonly ISalaryCalculationService _calculationService;

    /// <summary>
    ///     Инициализирует новый экземпляр контроллера <see cref="SalaryController"/>.
    /// </summary>
    /// <param name="service">Сервис для работы с зарплатными периодами.</param>
    /// <param name="calculationService">Сервис расчёта зарплатного периода.</param>
    public SalaryController(
        ISalaryService service,
        ISalaryCalculationService calculationService)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
        _calculationService = calculationService ?? throw new ArgumentNullException(nameof(calculationService));
    }

    /// <summary>
    ///     Получает подробную информацию о зарплатном периоде по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор зарплатного периода.</param>
    /// <returns>Детальная информация о зарплатном периоде.</returns>
    [HttpGet("{id:long}")]
    public Task<SalaryDto> GetAsync([FromRoute] long id)
    {
        return _service.GetAsync(id);
    }

    /// <summary>
    ///     Получает список зарплатных периодов с фильтрацией, сортировкой и пагинацией.
    /// </summary>
    /// <param name="request">Параметры запроса для фильтрации и сортировки зарплатных периодов.</param>
    /// <returns>Постраничный список кратких описаний зарплатных периодов.</returns>
    [HttpGet]
    public Task<PaginatedList<SalaryDto>> GetListAsync([FromQuery] GetSalaryListRequest request)
    {
        return _service.GetListAsync(request);
    }

    /// <summary>
    ///     Создаёт новый зарплатный период.
    /// </summary>
    /// <param name="request">Данные для создания зарплатного периода.</param>
    /// <returns>Созданный зарплатный период с полной информацией.</returns>
    [RequireRoles(UserRole.Editor, UserRole.Admin, UserRole.GuildLeader)]
    [HttpPost]
    public Task<SalaryDto> CreateAsync([FromBody] CreateSalaryRequest request)
    {
        return _service.CreateAsync(request);
    }

    /// <summary>
    ///     Обновляет существующий зарплатный период.
    /// </summary>
    /// <param name="id">Идентификатор зарплатного периода.</param>
    /// <param name="request">Данные для обновления зарплатного периода.</param>
    /// <returns>Обновлённый зарплатный период с полной информацией.</returns>
    [RequireRoles(UserRole.Editor, UserRole.Admin, UserRole.GuildLeader)]
    [HttpPut("{id:long}")]
    public Task<SalaryDto> UpdateAsync([FromRoute] long id, [FromBody] UpdateSalaryRequest request)
    {
        return _service.UpdateAsync(id, request);
    }

    /// <summary>
    ///     Завершает зарплатный период, изменяя статус заполнения на Filled.
    /// </summary>
    /// <param name="id">Идентификатор зарплатного периода.</param>
    /// <returns>Обновлённый зарплатный период с полной информацией.</returns>
    [RequireRoles(UserRole.Admin, UserRole.GuildLeader)]
    [HttpPost("{id:long}/finish")]
    public Task<SalaryDto> FinishAsync([FromRoute] long id)
    {
        return _service.FinishAsync(id);
    }

    /// <summary>
    ///     Открывает регистрацию на зарплатный период, изменяя статус регистрации на Opened.
    /// </summary>
    /// <param name="id">Идентификатор зарплатного периода.</param>
    /// <returns>Обновлённый зарплатный период с полной информацией.</returns>
    [RequireRoles(UserRole.Editor, UserRole.Admin, UserRole.GuildLeader)]
    [HttpPost("{id:long}/open-registration")]
    public Task<SalaryDto> OpenRegistrationAsync([FromRoute] long id)
    {
        return _service.OpenRegistrationAsync(id);
    }

    /// <summary>
    ///     Закрывает регистрацию на зарплатный период, изменяя статус регистрации на Closed.
    /// </summary>
    /// <param name="id">Идентификатор зарплатного периода.</param>
    /// <returns>Обновлённый зарплатный период с полной информацией.</returns>
    [RequireRoles(UserRole.Editor, UserRole.Admin, UserRole.GuildLeader)]
    [HttpPost("{id:long}/close-registration")]
    public Task<SalaryDto> CloseRegistrationAsync([FromRoute] long id)
    {
        return _service.CloseRegistrationAsync(id);
    }

    /// <summary>
    ///     Закрывает регистрацию на зарплатный период, изменяя статус регистрации на Closed.
    /// </summary>
    /// <param name="id">Идентификатор зарплатного периода.</param>
    /// <returns>Обновлённый зарплатный период с полной информацией.</returns>
    [RequireRoles(UserRole.Editor, UserRole.Admin, UserRole.GuildLeader)]
    [HttpPost("{id:long}/calculate")]
    public Task CalculateAsync([FromRoute] long id)
    {
        return _calculationService.CalculateAsync(id);
    }

    /// <summary>
    ///     Удаляет зарплатный период по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор зарплатного периода для удаления.</param>
    [RequireRoles(UserRole.Admin, UserRole.GuildLeader)]
    [HttpDelete("{id:long}")]
    public Task DeleteAsync([FromRoute] long id)
    {
        return _service.DeleteAsync(id);
    }
}
