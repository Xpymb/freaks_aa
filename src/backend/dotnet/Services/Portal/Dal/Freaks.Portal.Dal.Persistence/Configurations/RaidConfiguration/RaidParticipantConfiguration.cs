using Freaks.Portal.Contracts.Entities.RaidSummary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freaks.Portal.Dal.Persistence.Configurations.RaidConfiguration;

/// <inheritdoc />
public class RaidParticipantConfiguration : IEntityTypeConfiguration<RaidParticipant>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<RaidParticipant> builder)
    {
        builder.HasKey(p =>
                           new
                           {
                               p.RaidId, p.ParticipantId,
                           });

        builder
            .HasOne(p => p.Raid)
            .WithMany(r => r.Participants)
            .HasForeignKey(p => p.RaidId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(p => p.User)
            .WithOne()
            .HasForeignKey<RaidParticipant>(p => p.ParticipantId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}