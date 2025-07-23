namespace Freaks.Portal.SharedContracts.ValueObjects.RaidSummary;

/// <summary>
/// Перечисление типов боссов для рейдов.
/// Используется для указания конкретного босса, против которого будет идти рейд.
/// </summary>
public enum BossType
{
    /// <summary>
    /// АГЛ (Jmg).
    /// </summary>
    Jmg = 1,

    /// <summary>
    /// Т2 АГЛ (Abyssal Jmg).
    /// </summary>
    AbyssalJmg = 2,

    /// <summary>
    /// Марли (Rangora).
    /// </summary>
    Rangora = 3,

    /// <summary>
    /// Морфеус (Morpheus).
    /// </summary>
    Morpheus = 4,

    /// <summary>
    /// Кракен (Kraken).
    /// </summary>
    Kraken = 5,

    /// <summary>
    /// Чёрный дракон (Black Dragon).
    /// </summary>
    BlackDragon = 6,

    /// <summary>
    /// Калидис (Charybdis).
    /// </summary>
    Charybdis = 7,

    /// <summary>
    /// Левиафан (Leviathan).
    /// </summary>
    Leviathan = 8,

    /// <summary>
    /// Анталлон (Anthalon).
    /// </summary>
    Anthalon = 9,

    /// <summary>
    /// Кошка (Abyssal Sehekmet).
    /// </summary>
    AbyssalSehekmet = 10,
}