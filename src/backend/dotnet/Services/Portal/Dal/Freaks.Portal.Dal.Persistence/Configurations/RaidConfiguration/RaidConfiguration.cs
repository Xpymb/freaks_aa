using Freaks.Portal.Contracts.Entities.RaidSummary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freaks.Portal.Dal.Persistence.Configurations.RaidConfiguration;

/// <inheritdoc />
public class RaidConfiguration : IEntityTypeConfiguration<Raid>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Raid> builder)
    {
        builder
            .HasOne(r => r.Creator)
            .WithOne()
            .HasForeignKey<Raid>(r => r.CreatorId)
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
    }
}