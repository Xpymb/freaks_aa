using Freaks.Portal.Contracts.Entities.Shop;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freaks.Portal.Dal.Persistence.Configurations.Shop;

/// <inheritdoc />
public class ShopItemConfiguration : IEntityTypeConfiguration<ShopItem>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<ShopItem> builder)
    {
        builder
            .HasOne(i => i.LootItem)
            .WithMany()
            .HasForeignKey(i => i.LootItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(i => i.Creator)
            .WithMany()
            .HasForeignKey(i => i.CreatorId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasMany(i => i.Requests)
            .WithOne(r => r.Item)
            .HasForeignKey(r => r.ShopItemId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}