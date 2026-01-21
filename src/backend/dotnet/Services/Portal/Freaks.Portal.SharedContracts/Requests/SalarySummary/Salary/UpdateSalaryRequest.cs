using Freaks.Portal.SharedContracts.ValueObjects.RaidSummary;
using Freaks.Portal.SharedContracts.ValueObjects.SalarySummary;

namespace Freaks.Portal.SharedContracts.Requests.SalarySummary.Salary;

/// <summary>
///     Запрос на обновление информации о зарплатном периоде.
///     Содержит новое значение для названия.
/// </summary>
/// <param name="Name">Название зарплатного периода.</param>
/// <param name="From">Начало зарплатного периода.</param>
/// <param name="To">Конец зарплатного периода.</param>
/// <param name="AllowedPaymentTypes">Разрешенные типы выплат зарплаты.</param>
/// <param name="UseCoefficients">Использовать коэффициенты при расчете.</param>
/// <param name="BossTypes">Типы боссов для расчета зарплаты.</param>
public record UpdateSalaryRequest(
    string Name,
    DateOnly From,
    DateOnly To,
    IList<SalaryPaymentType> AllowedPaymentTypes,
    bool UseCoefficients,
    IList<BossType> BossTypes);
