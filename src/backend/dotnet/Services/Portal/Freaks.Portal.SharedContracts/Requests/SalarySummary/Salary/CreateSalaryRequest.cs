using Freaks.Portal.SharedContracts.ValueObjects.RaidSummary;
using Freaks.Portal.SharedContracts.ValueObjects.SalarySummary;

namespace Freaks.Portal.SharedContracts.Requests.SalarySummary.Salary;

/// <summary>
///     Запрос на создание нового зарплатного периода.
///     Содержит базовую информацию, необходимую для создания: название и даты начала/окончания.
/// </summary>
/// <param name="Name">Название зарплатного периода.</param>
/// <param name="StartDt">Дата начала периода.</param>
/// <param name="EndDt">Дата окончания периода.</param>
/// <param name="AllowedPaymentTypes">Разрешённые способы получения ЗП.</param>
/// <param name="UseCoefficients">Признак: использовать коэффициентную систему.</param>
/// <param name="BossTypes">Учитываемые боссы.</param>
public record CreateSalaryRequest(
    string Name,
    DateOnly StartDt,
    DateOnly EndDt,
    IList<SalaryPaymentType> AllowedPaymentTypes,
    bool UseCoefficients,
    IList<BossType> BossTypes);
