using Freaks.Portal.Contracts.Entities.RaidSummary;

namespace Freaks.Portal.Contracts.ValueObjects.RaidSummary;

public record RaidFullInfo(
    Raid Raid,
    List<RaidParticipant> Participants,
    List<RaidLoot> Loot);