using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Freaks.Contracts.Common.Interfaces;
using Freaks.Dal.Common.Consts;
using Freaks.Portal.Contracts.Entities.Loot;
using Freaks.Portal.SharedContracts.ValueObjects.Auction;
using Freaks.Users.Contracts.Entities;

namespace Freaks.Portal.Contracts.Entities.Auction;

/// <summary>
///     Представляет лот аукциона, содержащий информацию о предмете, начальной цене,
///     шаге ставки, сроке завершения и создателе.
/// </summary>
[Table("auction_item", Schema = DatabaseConsts.PortalSchema)]
public class AuctionItem : IEntity<long>
{
    /// <inheritdoc />
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public long Id { get; init; }

    /// <summary>
    ///     Идентификатор связанного предмета лута, выставленного на аукцион.
    /// </summary>
    [Column("loot_item_id")]
    public required int LootItemId { get; init; }

    /// <summary>
    ///     Начальная цена аукционного лота.
    /// </summary>
    [Column("start_price")]
    public required decimal StartPrice { get; init; }

    /// <summary>
    ///     Минимальный шаг увеличения ставки при торгах.
    /// </summary>
    [Column("min_price_step")]
    public required decimal MinPriceStep { get; set; }

    /// <summary>
    ///     Дата и время создания лота аукциона.
    /// </summary>
    [Column("created_dt")]
    public required DateTimeOffset CreatedDt { get; init; }
    
    /// <summary>
    ///     Дата и время завершения аукциона.
    /// </summary>
    [Column("end_dt")]
    public required DateTimeOffset EndDt { get; init; }

    /// <summary>
    ///     Статус лота на аукционе
    /// </summary>
    [Column("status")]
    public required AuctionItemStatus Status { get; set; }

    /// <summary>
    ///     Идентификатор пользователя, создавшего лот.
    /// </summary>
    [Column("creator_id")]
    public required Guid CreatorId { get; init; }

    /// <summary>
    ///     Текстовое описание лота.
    /// </summary>
    [Column("Description")]
    public required string Description { get; set; }

    /// <summary>
    ///     Навигационное свойство — предмет лута, связанный с этим лотом.
    /// </summary>
    public LootItem? LootItem { get; init; }

    /// <summary>
    ///     Навигационное свойство — пользователь, создавший лот.
    /// </summary>
    public User? Creator { get; init; }

    public IList<AuctionItemBid> Bids { get; init; } = new List<AuctionItemBid>();
}