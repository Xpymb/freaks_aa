using Freaks.Portal.Bll.Interfaces.SalarySummary;
using Freaks.Portal.SharedContracts.Dto.SalarySummary;
using Freaks.Users.Common.Attributes;
using Freaks.Users.Contracts.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freaks.Portal.WebApi.Controllers.SalarySummary;

/// <summary>
///     Контроллер для получения итогового отчёта зарплатного периода.
/// </summary>
[ApiController]
[Authorize]
[RequireRoles(UserRole.Member)]
[Route("salaries/{salaryId:long}/final-report")]
public class SalaryFinalReportController : ControllerBase
{
    private readonly ISalaryFinalReportService _service;

    /// <summary>
    ///     Инициализирует новый экземпляр контроллера для работы с итоговым отчётом зарплатного периода.
    /// </summary>
    /// <param name="service">Сервис для работы с итоговыми отчётами.</param>
    public SalaryFinalReportController(ISalaryFinalReportService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    ///     Получает итоговый отчёт по указанному зарплатному периоду.
    ///     Если отчёт ещё не существует — создаёт его и запускает расчёт.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <returns>Итоговый отчёт зарплатного периода.</returns>
    [HttpGet]
    public Task<SalaryFinalReportDto> GetAsync([FromRoute] long salaryId)
    {
        return _service.GetAsync(salaryId);
    }
}
