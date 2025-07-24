namespace Freaks.Portal.SharedContracts.ValueObjects.Loot;

/// <summary>
///     Перечисление типов добычи, выпадающей в рейдах и с мировых боссов.
///     Определяет, к какому виду предметов или усилений относится добыча.
/// </summary>
public enum LootType
{
    /// <summary>
    ///     Эссенция мирового босса: материалы для усиления экипировки мировых боссов.
    /// </summary>
    WorldBossInfusion = 1,

    /// <summary>
    ///     Эфенская эссенция: материалы для усиления эфенской экипировки.
    /// </summary>
    ErenorInfusion = 2,

    /// <summary>
    ///     Акхиумный кристалл: редкий кристалл для крафта.
    /// </summary>
    ArcheumCrystal = 3,

    /// <summary>
    ///     Экипировка мирового босса: готовые предметы экипировки с боссов.
    /// </summary>
    WorldBossEquipment = 4,

    /// <summary>
    ///     Сердце босса: редкий трофей, часто используемый для крафта или продажи.
    /// </summary>
    BossHearth = 5,

    /// <summary>
    ///     Гравировка: вставка в экипировку, дающая бонусы к характеристикам.
    /// </summary>
    Lunagem = 6,

    /// <summary>
    ///     Свиток пробуждения экипировки мирового босса.
    /// </summary>
    AwakeningScroll = 7,

    /// <summary>
    ///     Генетический материал, используется для выращивания дракона.
    /// </summary>
    DragonHeartshard = 8,

    /// <summary>
    ///     Глайдер.
    /// </summary>
    Glider = 9,

    /// <summary>
    ///     Золото: внутриигровая валюта.
    /// </summary>
    Gold = 10,

    /// <summary>
    ///     Другое.
    /// </summary>
    Other = 30,
}
