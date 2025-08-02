using System.Collections.Immutable;
using Freaks.Portal.Contracts.Entities.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freaks.Portal.Dal.Persistence.Configurations.Notification;

public class NotificationChannelMessageConfiguration :IEntityTypeConfiguration<NotificationChannelMessage>
{
    public void Configure(EntityTypeBuilder<NotificationChannelMessage> builder)
    {
        builder
            .HasOne(m => m.NotificationChannel)
            .WithMany(c =>c.Messages)
            .HasForeignKey(m => m.NotificationChannelId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}