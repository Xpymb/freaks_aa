using Freaks.Portal.SharedContracts.Dto.SalarySummary;
using Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryMember;

namespace Freaks.Portal.Bll.Interfaces.SalarySummary;

/// <summary>
///     Сервис для управления участниками зарплатных периодов.
///     Предоставляет методы для получения, добавления, обновления и удаления участников, связанных с конкретным зарплатным периодом.
/// </summary>
public interface ISalaryMemberService
{
    /// <summary>
    ///     Возвращает список участников за указанный зарплатный период.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <returns>Список участников в виде DTO.</returns>
    Task<IList<SalaryMemberDto>> GetListAsync(long salaryId);

    /// <summary>
    ///     Добавляет нового участника в зарплатный период (для Member).
    ///     UserId берётся из IUserContext - текущий пользователь создаёт запись для себя.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <param name="request">Запрос с информацией об участнике (без UserId).</param>
    /// <returns>Добавленный участник в виде DTO.</returns>
    Task<SalaryMemberDto> CreateAsync(long salaryId, CreateSalaryMemberRequest request);

    /// <summary>
    ///     Добавляет нового участника в зарплатный период (для Admin/Editor/GuildLeader).
    ///     Admin/Editor/GuildLeader могут создать участника для любого пользователя.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <param name="request">Запрос с информацией об участнике (включая UserId).</param>
    /// <returns>Добавленный участник в виде DTO.</returns>
    Task<SalaryMemberDto> CreateAdminAsync(long salaryId, CreateSalaryMemberAdminRequest request);

    /// <summary>
    ///     Обновляет информацию об участнике в зарплатном периоде.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <param name="userId">Идентификатор пользователя (часть составного ключа).</param>
    /// <param name="request">Запрос с новой информацией об участнике.</param>
    /// <returns>Обновлённый участник в виде DTO.</returns>
    Task<SalaryMemberDto> UpdateAsync(long salaryId, Guid userId, UpdateSalaryMemberRequest request);

    /// <summary>
    ///     Удаляет запись об участнике из зарплатного периода.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <param name="userId">Идентификатор пользователя (часть составного ключа) для удаления.</param>
    Task DeleteAsync(long salaryId, Guid userId);
}
