namespace Freaks.Portal.Bll.Interfaces.SalarySummary;

/// <summary>
///     Сервис для расчета зарплат участников за зарплатный период.
///     Выполняет расчет на основе рейдов, лута, расходов и участников.
/// </summary>
public interface ISalaryCalculationService
{
    /// <summary>
    ///     Выполняет расчет зарплат для всех участников зарплатного периода.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    Task CalculateAsync(long salaryId);
}