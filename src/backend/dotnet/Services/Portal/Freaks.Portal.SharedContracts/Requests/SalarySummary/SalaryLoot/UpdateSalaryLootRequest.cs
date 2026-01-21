namespace Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryLoot;

/// <summary>
///     Запрос на обновление проданного лута в зарплатном периоде.
///     Поддерживает обратный пересчёт: если передан DiscountPercent, пересчитывается Amount и наоборот.
/// </summary>
/// <param name="LootId">Идентификатор предмета лута (из справочника).</param>
/// <param name="Quantity">Количество проданных предметов.</param>
/// <param name="PricePerItem">Цена за единицу лута.</param>
/// <param name="DiscountPercent">Скидка в процентах. Если передан, пересчитывается Amount.</param>
/// <param name="Amount">Итоговая сумма. Если передан, пересчитывается DiscountPercent. Если переданы оба параметра, приоритет у DiscountPercent.</param>
public record UpdateSalaryLootRequest(
    int LootId,
    int Quantity,
    decimal PricePerItem,
    decimal DiscountPercent,
    decimal Amount);
