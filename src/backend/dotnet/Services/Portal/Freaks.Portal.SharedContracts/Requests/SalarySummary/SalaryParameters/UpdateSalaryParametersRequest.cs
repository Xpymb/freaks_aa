using Freaks.Portal.SharedContracts.ValueObjects.RaidSummary;
using Freaks.Portal.SharedContracts.ValueObjects.SalarySummary;

namespace Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryParameters;

/// <summary>
///     Запрос на обновление параметров зарплатного периода.
///     Позволяет изменить все настройки расчета зарплаты.
/// </summary>
/// <param name="AllowedPaymentTypes">Разрешенные типы выплат зарплаты.</param>
/// <param name="UseCoefficients">Использовать коэффициенты при расчете.</param>
/// <param name="BossTypes">Типы боссов для расчета зарплаты.</param>
public record UpdateSalaryParametersRequest(
    IList<SalaryPaymentType> AllowedPaymentTypes,
    bool UseCoefficients,
    IList<BossType> BossTypes);
