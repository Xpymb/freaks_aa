namespace Freaks.Portal.SharedContracts.ValueObjects.Loot;

/// <summary>
/// Перечисление типов добычи, выпадающей в рейдах и с мировых боссов.
/// Определяет, к какому виду предметов или усилений относится добыча.
/// </summary>
public enum LootType
{
    /// <summary>
    /// Эссенция мирового босса: материалы для усиления экипировки мировых боссов.
    /// </summary>
    WorldBossInfusion = 1,

    /// <summary>
    /// Эссенция Эфена: материалы для усиления экипировки Эфена.
    /// </summary>
    ErenorInfusion = 2,

    /// <summary>
    /// Акхиумный кристалл: редкий кристалл для крафта.
    /// </summary>
    ArcheumCrystal = 3,

    /// <summary>
    /// Экипировка мирового босса: готовые предметы экипировки с боссов.
    /// </summary>
    WorldBossEquipment = 4,

    /// <summary>
    /// Сердце босса.
    /// </summary>
    BossHearth = 5,
}