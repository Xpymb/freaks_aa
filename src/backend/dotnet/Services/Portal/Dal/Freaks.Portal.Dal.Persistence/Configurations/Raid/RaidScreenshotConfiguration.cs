using Freaks.Portal.Contracts.Entities.RaidSummary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freaks.Portal.Dal.Persistence.Configurations.Raid;

/// <inheritdoc />
public class RaidScreenshotConfiguration : IEntityTypeConfiguration<RaidScreenshot>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<RaidScreenshot> builder)
    {
        builder.HasKey(s =>
                           new
                           {
                               s.RaidId, ScreenshotUrl = s.ScreenshotUri,
                           });

        builder
            .HasOne(s => s.Raid)
            .WithMany(r => r.Screenshots)
            .HasForeignKey(r => r.RaidId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}