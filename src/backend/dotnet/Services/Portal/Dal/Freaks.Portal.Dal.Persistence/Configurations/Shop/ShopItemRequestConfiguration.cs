using Freaks.Portal.Contracts.Entities.Shop;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freaks.Portal.Dal.Persistence.Configurations.Shop;

/// <inheritdoc />
public class ShopItemRequestConfiguration : IEntityTypeConfiguration<ShopItemRequest>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<ShopItemRequest> builder)
    {
        builder.HasKey(r =>
                           new
                           {
                               r.ShopItemId, r.UserId,
                           });

        builder
            .HasOne(r => r.Item)
            .WithMany(i => i.Requests)
            .HasForeignKey(r => r.ShopItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(r => r.User)
            .WithMany()
            .HasForeignKey(r => r.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}