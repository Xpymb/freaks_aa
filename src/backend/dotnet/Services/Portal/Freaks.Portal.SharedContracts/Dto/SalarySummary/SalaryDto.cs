using Freaks.Portal.SharedContracts.ValueObjects.RaidSummary;
using Freaks.Portal.SharedContracts.ValueObjects.SalarySummary;

namespace Freaks.Portal.SharedContracts.Dto.SalarySummary;

/// <summary>
///     Полная информация о зарплатном периоде.
///     Используется для детального представления зарплатного периода.
/// </summary>
/// <param name="Id">Уникальный идентификатор зарплатного периода.</param>
/// <param name="Name">Название зарплатного периода.</param>
/// <param name="StartDt">Дата начала периода.</param>
/// <param name="EndDt">Дата окончания периода.</param>
/// <param name="FillStepType">Статус заполнения зарплаты.</param>
/// <param name="RegistrationStatus">Статус регистрации на зарплату.</param>
/// <param name="AllowedPaymentTypes">Разрешённые способы получения ЗП.</param>
/// <param name="UseCoefficients">Признак: использовать коэффициентную систему.</param>
/// <param name="BossTypes">Учитываемые боссы.</param>
public record SalaryDto(
    long Id,
    string Name,
    DateOnly StartDt,
    DateOnly EndDt,
    SalaryFillStepType FillStepType,
    SalaryRegistrationStatus RegistrationStatus,
    IList<SalaryPaymentType> AllowedPaymentTypes,
    bool UseCoefficients,
    IList<BossType> BossTypes);
