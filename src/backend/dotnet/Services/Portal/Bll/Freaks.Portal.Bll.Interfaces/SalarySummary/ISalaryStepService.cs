using Freaks.Portal.SharedContracts.ValueObjects.SalarySummary;

namespace Freaks.Portal.Bll.Interfaces.SalarySummary;

/// <summary>
///     Сервис для управления шагами заполнения зарплатного периода.
///     Контролирует переходы между этапами и проверяет права доступа.
/// </summary>
public interface ISalaryStepService
{
    /// <summary>
    ///     Обрабатывает действие по переходу к следующему шагу заполнения зарплатного периода.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <param name="fillStepType">Тип шага заполнения.</param>
    Task HandleStepActionAsync(long salaryId, SalaryFillStepType fillStepType);

    /// <summary>
    ///     Проверяет права доступа для выполнения действия над зарплатным периодом.
    /// </summary>
    /// <param name="salaryId">Идентификатор зарплатного периода.</param>
    /// <param name="actionType">Тип действия (заполнение или регистрация).</param>
    Task CheckAccessAsync(long salaryId, SalaryActionType actionType);
}