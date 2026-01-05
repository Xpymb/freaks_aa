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
/// <param name="FillStatus">Статус заполнения зарплаты.</param>
/// <param name="RegistrationStatus">Статус регистрации на зарплату.</param>
public record SalaryDto(
    long Id,
    string Name,
    DateOnly StartDt,
    DateOnly EndDt,
    SalaryFillStatus FillStatus,
    SalaryRegistrationStatus RegistrationStatus);
