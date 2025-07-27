using Freaks.Portal.Contracts.Entities.Auction;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freaks.Portal.Dal.Persistence.Configurations.Auction;

/// <inheritdoc />
public class AuctionItemBidConfiguration : IEntityTypeConfiguration<AuctionItemBid>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<AuctionItemBid> builder)
    {
        builder
            .HasOne(b => b.Item)
            .WithMany(i => i.Bids)
            .HasForeignKey(b => b.AuctionItemId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(b => b.Creator)
            .WithMany()
            .HasForeignKey(b => b.CreatorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}