using Freaks.Portal.SharedContracts.Dto.Loot;

namespace Freaks.Portal.SharedContracts.Dto.SalarySummary;

/// <summary>
///     DTO, представляющее проданный лут за зарплатный период.
/// </summary>
/// <param name="Id">Уникальный идентификатор записи.</param>
/// <param name="SalaryId">Идентификатор зарплатного периода.</param>
/// <param name="Loot">Информация о предмете лута.</param>
/// <param name="Quantity">Количество проданных предметов.</param>
/// <param name="PricePerLoot">Цена за единицу лута.</param>
/// <param name="DiscountPercent">Скидка в процентах.</param>
/// <param name="Amount">Итоговая сумма.</param>
public record SalaryLootDto(
    long Id,
    long SalaryId,
    LootItemDto Loot,
    int Quantity,
    decimal PricePerLoot,
    decimal DiscountPercent,
    decimal Amount);
