using Freaks.Portal.SharedContracts.ValueObjects.RaidSummary;
using Freaks.Portal.SharedContracts.ValueObjects.SalarySummary;

namespace Freaks.Portal.SharedContracts.Dto.SalarySummary;

/// <summary>
///     DTO для параметров зарплатного периода.
///     Содержит настройки для расчета зарплаты: типы выплат, коэффициенты и список боссов.
/// </summary>
/// <param name="Id">Идентификатор зарплатного периода.</param>
/// <param name="AllowedPaymentTypes">Разрешенные типы выплат зарплаты.</param>
/// <param name="UseCoefficients">Использовать коэффициенты при расчете.</param>
/// <param name="BossTypes">Типы боссов для расчета зарплаты.</param>
public record SalaryParametersDto(
    long Id,
    IList<SalaryPaymentType> AllowedPaymentTypes,
    bool UseCoefficients,
    IList<BossType> BossTypes);
