using Freaks.Portal.SharedContracts.Dto.SalarySummary;

namespace Freaks.Portal.Bll.Interfaces.SalarySummary;

/// <summary>
///     Интерфейс сервиса для работы с итоговым отчётом зарплатного периода.
/// </summary>
public interface ISalaryFinalReportService
{
    /// <summary>
    ///     Получает итоговый отчёт по зарплатному периоду.
    ///     Если отчёт ещё не существует — создаёт его и запускает расчёт.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <returns>DTO итогового отчёта.</returns>
    Task<SalaryFinalReportDto> GetAsync(long salaryId);
}