using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freaks.Portal.Dal.Persistence.Configurations.Raid;

/// <inheritdoc />
public class RaidConfiguration : IEntityTypeConfiguration<Contracts.Entities.RaidSummary.Raid>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Contracts.Entities.RaidSummary.Raid> builder)
    {
        builder
            .HasOne(r => r.Creator)
            .WithMany()
            .HasForeignKey(r => r.CreatorId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder
            .HasMany(r => r.Participants)
            .WithOne(p => p.Raid)
            .HasForeignKey(p => p.RaidId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(r => r.Screenshots)
            .WithOne(s => s.Raid)
            .HasForeignKey(s => s.RaidId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(r => r.Loots)
            .WithOne(p => p.Raid)
            .HasForeignKey(p => p.RaidId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasIndex(r => new { r.StartDt, r.BossType });
    }
}