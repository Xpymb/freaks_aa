using Freaks.Portal.Bll.Interfaces.SalarySummary;
using Freaks.Portal.SharedContracts.Dto.SalarySummary;
using Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryMember;
using Freaks.Users.Common.Attributes;
using Freaks.Users.Contracts.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freaks.Portal.WebApi.Controllers.SalarySummary;

/// <summary>
///     Контроллер для управления участниками зарплатных периодов.
///     Позволяет просматривать, добавлять, изменять и удалять участников.
/// </summary>
[ApiController]
[Authorize]
[RequireRoles(UserRole.Member)]
[Route("salaries/{salaryId:long}/members")]
public class SalaryMemberController : ControllerBase
{
    private readonly ISalaryMemberService _service;

    /// <summary>
    ///     Инициализирует новый экземпляр контроллера для управления участниками зарплатного периода.
    /// </summary>
    /// <param name="service">Сервис для работы с участниками зарплатного периода.</param>
    public SalaryMemberController(ISalaryMemberService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    ///     Получает список участников, связанных с указанным зарплатным периодом.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <returns>Список участников в зарплатном периоде.</returns>
    [HttpGet]
    public Task<IList<SalaryMemberDto>> GetListAsync([FromRoute] long salaryId)
    {
        return _service.GetListAsync(salaryId);
    }

    /// <summary>
    ///     Добавляет новую запись об участнике в указанный зарплатный период (для Member).
    ///     UserId берётся из IUserContext - текущий пользователь создаёт запись для себя.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <param name="request">Данные нового участника (без UserId).</param>
    /// <returns>Информация о добавленном участнике.</returns>
    [HttpPost]
    public Task<SalaryMemberDto> CreateAsync([FromRoute] long salaryId, [FromBody] CreateSalaryMemberRequest request)
    {
        return _service.CreateAsync(salaryId, request);
    }

    /// <summary>
    ///     Добавляет новую запись об участнике в указанный зарплатный период (для Admin/Editor/GuildLeader).
    ///     Admin/Editor/GuildLeader могут создать участника для любого пользователя.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <param name="request">Данные нового участника (включая UserId).</param>
    /// <returns>Информация о добавленном участнике.</returns>
    [RequireRoles(UserRole.Admin, UserRole.Editor, UserRole.GuildLeader)]
    [HttpPost("admin")]
    public Task<SalaryMemberDto> CreateAdminAsync([FromRoute] long salaryId, [FromBody] CreateSalaryMemberAdminRequest request)
    {
        return _service.CreateAdminAsync(salaryId, request);
    }

    /// <summary>
    ///     Обновляет информацию об участнике в указанном зарплатном периоде.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <param name="userId">Идентификатор пользователя (часть составного ключа).</param>
    /// <param name="request">Обновлённые данные об участнике.</param>
    /// <returns>Информация об обновлённом участнике.</returns>
    [RequireRoles(UserRole.Admin, UserRole.Editor, UserRole.GuildLeader)]
    [HttpPut("{userId:guid}")]
    public Task<SalaryMemberDto> UpdateAsync([FromRoute] long salaryId, [FromRoute] Guid userId, [FromBody] UpdateSalaryMemberRequest request)
    {
        return _service.UpdateAsync(salaryId, userId, request);
    }

    /// <summary>
    ///     Удаляет запись об участнике из указанного зарплатного периода.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <param name="userId">Идентификатор пользователя (часть составного ключа).</param>
    /// <returns>Результат выполнения операции.</returns>
    [RequireRoles(UserRole.Admin, UserRole.Editor, UserRole.GuildLeader)]
    [HttpDelete("{userId:guid}")]
    public Task DeleteAsync([FromRoute] long salaryId, [FromRoute] Guid userId)
    {
        return _service.DeleteAsync(salaryId, userId);
    }
}
