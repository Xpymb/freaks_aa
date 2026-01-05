using Freaks.Portal.Bll.Interfaces.SalarySummary;
using Freaks.Portal.SharedContracts.Dto.SalarySummary;
using Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryParameters;
using Freaks.Users.Common.Attributes;
using Freaks.Users.Contracts.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freaks.Portal.WebApi.Controllers.SalarySummary;

/// <summary>
///     Контроллер для управления параметрами зарплатных периодов.
///     Позволяет просматривать и изменять настройки расчета зарплаты.
/// </summary>
[ApiController]
[Authorize]
[RequireRoles(UserRole.Member)]
[Route("salaries/{salaryId:long}/parameters")]
public class SalaryParametersController : ControllerBase
{
    private readonly ISalaryParametersService _service;

    /// <summary>
    ///     Инициализирует новый экземпляр контроллера для управления параметрами зарплатного периода.
    /// </summary>
    /// <param name="service">Сервис для работы с параметрами зарплатного периода.</param>
    public SalaryParametersController(ISalaryParametersService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    ///     Получает параметры указанного зарплатного периода.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <returns>Параметры зарплатного периода.</returns>
    [HttpGet]
    public async Task<SalaryParametersDto> GetAsync([FromRoute] long salaryId)
    {
        return await _service.GetAsync(salaryId);
    }

    /// <summary>
    ///     Обновляет параметры указанного зарплатного периода.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <param name="request">Новые параметры зарплатного периода.</param>
    /// <returns>Обновленные параметры зарплатного периода.</returns>
    [RequireRoles(UserRole.Admin, UserRole.Editor, UserRole.GuildLeader)]
    [HttpPut]
    public async Task<SalaryParametersDto> UpdateAsync(
        [FromRoute] long salaryId,
        [FromBody] UpdateSalaryParametersRequest request)
    {
        return await _service.UpdateAsync(salaryId, request);
    }
}
