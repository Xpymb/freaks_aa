using Freaks.Portal.SharedContracts.Dto.SalarySummary;
using Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryParameters;

namespace Freaks.Portal.Bll.Interfaces.SalarySummary;

/// <summary>
///     Сервис для управления параметрами зарплатных периодов.
///     Предоставляет методы для получения и обновления настроек расчета зарплаты.
/// </summary>
public interface ISalaryParametersService
{
    /// <summary>
    ///     Получает параметры зарплатного периода.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <returns>Параметры зарплатного периода в виде DTO.</returns>
    Task<SalaryParametersDto> GetAsync(long salaryId);

    /// <summary>
    ///     Обновляет параметры зарплатного периода.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <param name="request">Запрос с новыми параметрами.</param>
    /// <returns>Обновленные параметры зарплатного периода в виде DTO.</returns>
    Task<SalaryParametersDto> UpdateAsync(long salaryId, UpdateSalaryParametersRequest request);
}
