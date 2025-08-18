using Freaks.Portal.Contracts.Entities.Notification;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freaks.Portal.Dal.Persistence.Configurations.Notification;

/// <inheritdoc />
public class NotificationChannelConfiguration : IEntityTypeConfiguration<NotificationChannel>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<NotificationChannel> builder)
    {
        builder
            .HasMany(c => c.Messages) 
            .WithOne(m => m.NotificationChannel) 
            .HasForeignKey(m => m.NotificationChannelId) 
            .OnDelete(DeleteBehavior.Cascade); 
    }
}