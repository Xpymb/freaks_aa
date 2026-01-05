using Freaks.Portal.SharedContracts.Dto.SalarySummary;
using Freaks.Portal.SharedContracts.Requests.SalarySummary.Salary;
using Freaks.SharedContracts.Common;

namespace Freaks.Portal.Bll.Interfaces.SalarySummary;

/// <summary>
///     Сервис для управления зарплатными периодами и выполнения CRUD-операций.
///     Отвечает за бизнес-логику, связанную с созданием, редактированием, удалением и получением зарплатных периодов.
/// </summary>
public interface ISalaryService
{
    /// <summary>
    ///     Получает подробную информацию о зарплатном периоде по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор зарплатного периода.</param>
    /// <returns>Детальная информация о зарплатном периоде.</returns>
    Task<SalaryDto> GetAsync(long id);

    /// <summary>
    ///     Получает список зарплатных периодов с фильтрацией, сортировкой и пагинацией.
    /// </summary>
    /// <param name="request">Параметры запроса для фильтрации и сортировки зарплатных периодов.</param>
    /// <returns>Постраничный список кратких описаний зарплатных периодов.</returns>
    Task<PaginatedList<SalaryDto>> GetListAsync(GetSalaryListRequest request);

    /// <summary>
    ///     Создаёт новый зарплатный период.
    /// </summary>
    /// <param name="request">Данные для создания зарплатного периода.</param>
    /// <returns>Созданный зарплатный период с полной информацией.</returns>
    Task<SalaryDto> CreateAsync(CreateSalaryRequest request);

    /// <summary>
    ///     Обновляет существующий зарплатный период.
    /// </summary>
    /// <param name="id">Идентификатор зарплатного периода.</param>
    /// <param name="request">Данные для обновления зарплатного периода.</param>
    /// <returns>Обновлённый зарплатный период с полной информацией.</returns>
    Task<SalaryDto> UpdateAsync(long id, UpdateSalaryRequest request);

    /// <summary>
    ///     Завершает зарплатный период, изменяя статус заполнения на Filled.
    /// </summary>
    /// <param name="id">Идентификатор зарплатного периода.</param>
    /// <returns>Обновлённый зарплатный период с полной информацией.</returns>
    Task<SalaryDto> FinishAsync(long id);

    /// <summary>
    ///     Открывает регистрацию на зарплатный период, изменяя статус регистрации на Opened.
    /// </summary>
    /// <param name="id">Идентификатор зарплатного периода.</param>
    /// <returns>Обновлённый зарплатный период с полной информацией.</returns>
    Task<SalaryDto> OpenRegistrationAsync(long id);

    /// <summary>
    ///     Закрывает регистрацию на зарплатный период, изменяя статус регистрации на Closed.
    /// </summary>
    /// <param name="id">Идентификатор зарплатного периода.</param>
    /// <returns>Обновлённый зарплатный период с полной информацией.</returns>
    Task<SalaryDto> CloseRegistrationAsync(long id);

    /// <summary>
    ///     Удаляет зарплатный период по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор зарплатного периода для удаления.</param>
    Task DeleteAsync(long id);
}
