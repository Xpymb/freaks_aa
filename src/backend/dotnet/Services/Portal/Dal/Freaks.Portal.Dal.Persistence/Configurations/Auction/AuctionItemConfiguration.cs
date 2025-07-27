using Freaks.Portal.Contracts.Entities.Auction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freaks.Portal.Dal.Persistence.Configurations.Auction;

/// <inheritdoc />
public class AuctionItemConfiguration : IEntityTypeConfiguration<AuctionItem>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<AuctionItem> builder)
    {
        builder
            .HasMany(i => i.Bids)
            .WithOne(b => b.Item)
            .HasForeignKey(b => b.AuctionItemId)
            .OnDelete(DeleteBehavior.Cascade);

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
    }
}