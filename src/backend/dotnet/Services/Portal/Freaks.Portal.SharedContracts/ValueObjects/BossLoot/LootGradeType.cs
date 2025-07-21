namespace Freaks.Portal.SharedContracts.ValueObjects.BossLoot;

/// <summary>
/// Перечисление рангов (уровней редкости) добычи в рейдах и у мировых боссов.
/// Используется для определения редкости и класса предмета.
/// </summary>
public enum LootGradeType
{
    /// <summary>
    /// Бесполезный предмет.
    /// </summary>
    Crude = 1,

    /// <summary>
    /// Обычный предмет.
    /// </summary>
    Basic = 2,

    /// <summary>
    /// Необычный предмет.
    /// </summary>
    Grand = 3,

    /// <summary>
    /// Редкий предмет.
    /// </summary>
    Rare = 4,

    /// <summary>
    /// Уникальный предмет.
    /// </summary>
    Arcane = 5,

    /// <summary>
    /// Эпический предмет.
    /// </summary>
    Heroic = 6,

    /// <summary>
    /// Легендарный предмет.
    /// </summary>
    Unique = 7,

    /// <summary>
    /// Реликвия.
    /// </summary>
    Celestial = 8,

    /// <summary>
    /// Предмет эпохи чудес.
    /// </summary>
    Divine = 9,

    /// <summary>
    /// Предмет эпохи сказаний.
    /// </summary>
    Epic = 10,

    /// <summary>
    /// Предмет эпохи легенд.
    /// </summary>
    Legendary = 11,

    /// <summary>
    /// Предмет эпохи мифов.
    /// </summary>
    Mythic = 12,

    /// <summary>
    /// Предмет эпохи Двенадцати.
    /// </summary>
    Eternal = 13,
}