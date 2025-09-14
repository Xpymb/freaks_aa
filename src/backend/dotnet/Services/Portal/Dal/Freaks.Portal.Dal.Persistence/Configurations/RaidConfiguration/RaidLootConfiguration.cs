using Freaks.Portal.Contracts.Entities.RaidSummary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freaks.Portal.Dal.Persistence.Configurations.RaidConfiguration;

/// <inheritdoc />
public class RaidLootConfiguration : IEntityTypeConfiguration<RaidLoot>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<RaidLoot> builder)
    {
        builder.HasKey(l =>
                           new
                           {
                               l.RaidId, LootId = l.LootItemId,
                           });

        builder
            .HasOne(l => l.Raid)
            .WithMany(r => r.Loots)
            .HasForeignKey(l => l.RaidId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(l => l.Loot)
            .WithMany()
            .HasForeignKey(l => l.LootItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}