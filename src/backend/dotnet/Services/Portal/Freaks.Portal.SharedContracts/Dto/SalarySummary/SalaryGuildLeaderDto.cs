using Freaks.Portal.SharedContracts.Dto.Loot;

namespace Freaks.Portal.SharedContracts.Dto.SalarySummary;

/// <summary>
///     DTO, представляющее долю руководства гильдии в зарплатном периоде.
/// </summary>
/// <param name="Id">Уникальный идентификатор записи.</param>
/// <param name="SalaryId">Идентификатор зарплатного периода.</param>
/// <param name="Loot">Информация о предмете лута.</param>
/// <param name="Quantity">Количество предметов.</param>
/// <param name="PricePerItem">Цена за единицу лута.</param>
/// <param name="Amount">Итоговая сумма.</param>
public record SalaryGuildLeaderDto(
    long Id,
    long SalaryId,
    LootItemDto Loot,
    int Quantity,
    decimal PricePerItem,
    decimal Amount);
