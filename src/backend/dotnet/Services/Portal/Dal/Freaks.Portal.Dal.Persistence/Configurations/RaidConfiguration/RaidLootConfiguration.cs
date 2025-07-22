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
                               l.RaidId, l.LootId,
                           });

        builder
            .HasOne(l => l.Raid)
            .WithMany(r => r.Loots)
            .HasForeignKey(l => l.RaidId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(l => l.Loot)
            .WithOne()
            .HasForeignKey<RaidLoot>(l => l.LootId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}