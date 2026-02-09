namespace Freaks.Portal.SharedContracts.Dto.SalarySummary;

/// <summary>
///     DTO, представляющее долю руководства гильдии в зарплатном периоде.
/// </summary>
/// <param name="SalaryLoot">Информация о предмете лута зарплатного периода.</param>
/// <param name="SalaryId">Идентификатор зарплатного периода.</param>
/// <param name="Quantity">Количество предметов.</param>
/// <param name="Amount">Итоговая сумма.</param>
public record SalaryGuildLeaderDto(
    SalaryLootDto SalaryLoot,
    long SalaryId,
    int Quantity,
    decimal Amount);