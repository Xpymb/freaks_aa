using Freaks.Portal.Contracts.Entities.SalarySummary;

namespace Freaks.Portal.Bll.Implementation.SalarySummary.Helpers;

/// <summary>
///     Вспомогательный класс для расчётов, связанных с долей руководства гильдии.
/// </summary>
public static class SalaryGuildLeaderHelper
{
    /// <summary>
    ///     Рассчитывает итоговую сумму для доли руководства гильдии на основе проданного лута и количества.
    /// </summary>
    /// <param name="salaryLoot">Информация о проданном луте зарплатного периода.</param>
    /// <param name="quantity">Количество предметов для расчёта.</param>
    /// <returns>Рассчитанная сумма. Возвращает 0, если данные о луте отсутствуют.</returns>
    public static decimal CalculateAmount(SalaryLoot? salaryLoot, int quantity)
    {
        var result = salaryLoot?.Amount / salaryLoot?.Quantity * quantity;

        return result.GetValueOrDefault(0);
    }
}