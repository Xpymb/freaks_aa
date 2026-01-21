namespace Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryLoot;

/// <summary>
///     Запрос на добавление проданного лута в зарплатный период.
///     Amount вычисляется автоматически на основе переданных параметров.
/// </summary>
/// <param name="LootId">Идентификатор предмета лута (из справочника).</param>
/// <param name="Quantity">Количество проданных предметов.</param>
/// <param name="PricePerItem">Цена за единицу лута.</param>
/// <param name="DiscountPercent">Скидка в процентах.</param>
/// <param name="Amount">Итоговая сумма.</param>
public record CreateSalaryLootRequest(
    int LootId,
    int Quantity,
    decimal PricePerItem,
    decimal? DiscountPercent,
    decimal? Amount);
