using Freaks.Portal.SharedContracts.ValueObjects.Loot;

namespace Freaks.Portal.SharedContracts.Dto.Loot;

/// <summary>
///     DTO, представляющее предмет лута, получаемый с босса.
///     Содержит информацию о типе, качестве и названии предмета, а также синтез-опыте (если применимо).
/// </summary>
/// <param name="Id">Уникальный идентификатор предмета лута.</param>
/// <param name="Type">Тип предмета (например, оружие, броня, украшение и т.д.).</param>
/// <param name="GradeType">Качество/грейд предмета (например, Rare, Epic, Legendary).</param>
/// <param name="Name">Название предмета.</param>
/// <param name="Description">Описание предмета.</param>
/// <param name="SynthesisExp">Количество опыта для синтеза, если применимо (может быть null).</param>
public record BossLootDto(
    int Id,
    LootType Type,
    LootGradeType GradeType,
    string Name,
    string Description,
    int? SynthesisExp);