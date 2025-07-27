using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Freaks.Contracts.Common.Interfaces;
using Freaks.Dal.Common.Consts;
using Freaks.Users.Contracts.Entities;

namespace Freaks.Portal.Contracts.Entities.Auction;

/// <summary>
///     Представляет собой ставку, сделанную пользователем на лот аукциона.
/// </summary>
[Table("auction_item_bid", Schema = DatabaseConsts.PortalSchema)]
public class AuctionItemBid : IEntity<long>
{
    /// <summary>
    ///     Уникальный идентификатор ставки.
    /// </summary>
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public long Id { get; init; }

    /// <summary>
    ///     Идентификатор лота аукциона, по которому сделана ставка.
    /// </summary>
    [Column("auction_item_id")]
    public required long AuctionItemId { get; init; }

    /// <summary>
    ///     Идентификатор пользователя, сделавшего ставку.
    /// </summary>
    [Column("creator_id")]
    public required Guid CreatorId { get; init; }

    /// <summary>
    ///     Сумма ставки.
    /// </summary>
    [Column("price")]
    public required decimal Price { get; init; }

    /// <summary>
    ///     Дата и время создания ставки.
    /// </summary>
    [Column("created_dt")]
    public required DateTimeOffset CreatedDt { get; init; }

    /// <summary>
    ///     Навигационное свойство лота, по которому сделана ставка.
    /// </summary>
    public AuctionItem? Item { get; init; }

    /// <summary>
    ///     Навигационное свойство пользователя, сделавшего ставку.
    /// </summary>
    public User? Creator { get; init; }
}