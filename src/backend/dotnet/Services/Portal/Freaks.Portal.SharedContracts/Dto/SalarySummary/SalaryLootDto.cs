using Freaks.Portal.SharedContracts.Dto.Loot;

namespace Freaks.Portal.SharedContracts.Dto.SalarySummary;

/// <summary>
///     DTO, представляющее проданный лут за зарплатный период.
/// </summary>
/// <param name="Id">Уникальный идентификатор записи.</param>
/// <param name="SalaryId">Идентификатор зарплатного периода.</param>
/// <param name="LootItem">Информация о предмете лута.</param>
/// <param name="Quantity">Количество проданных предметов.</param>
/// <param name="PricePerItem">Цена за единицу лута.</param>
/// <param name="DiscountPercent">Скидка в процентах.</param>
/// <param name="Amount">Итоговая сумма.</param>
public record SalaryLootDto(
    long Id,
    long SalaryId,
    LootItemDto LootItem,
    int Quantity,
    decimal PricePerItem,
    decimal DiscountPercent,
    decimal Amount);
