using System.ComponentModel.DataAnnotations.Schema;
using Freaks.Contracts.Common.Interfaces;
using Freaks.Dal.Common.Consts;

namespace Freaks.Portal.Contracts.Entities.Auction;

/// <summary>
///     Представляет лот аукциона, содержащий информацию о предмете, начальной цене,
///     шаге ставки, сроке завершения и создателе.
/// </summary>
[Table("auction", Schema = DatabaseConsts.PortalSchema)]
public class AuctionItem : IEntity<int>
{
    /// <inheritdoc />
    [Column("id")]
    public int Id { get; init; }

    /// <summary>
    ///     Идентификатор связанного предмета лута, выставленного на аукцион.
    /// </summary>
    [Column("loot_item_id")]
    public required int LootItemId { get; init; }

    /// <summary>
    ///     Начальная цена аукционного лота.
    /// </summary>
    [Column("start_price")]
    public required int StartPrice { get; init; }

    /// <summary>
    ///     Минимальный шаг увеличения ставки при торгах.
    /// </summary>
    [Column("min_price_step")]
    public required int MinPriceStep { get; init; }

    /// <summary>
    ///     Дата и время завершения аукциона.
    /// </summary>
    [Column("end_dt")]
    public required DateTimeOffset EndDt { get; init; }

    /// <summary>
    ///     Идентификатор пользователя, создавшего лот.
    /// </summary>
    [Column("creator_id")]
    public required Guid CreatorId { get; init; }

    /// <summary>
    ///     Текстовое описание лота.
    /// </summary>
    [Column("Description")]
    public required string Description { get; init; }
}