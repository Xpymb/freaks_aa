using Freaks.Dal.Common.Interfaces;
using Freaks.Portal.Contracts.Entities.Auction;
using Freaks.Portal.Dal.Persistence;

namespace Freaks.Portal.Dal.Interfaces.Auction;

/// <summary>
///     Интерфейс провайдера для работы со ставками по лотам аукциона в контексте портального БД.
/// </summary>
public interface IAuctionItemBidProvider : IBaseProvider<AuctionItemBid, long, IPortalDbContext>
{
    /// <summary>
    ///     Получает список ставок для указанного лота аукциона.
    /// </summary>
    /// <param name="auctionItemId">Идентификатор лота, для которого необходимо получить ставки.</param>
    /// <returns>Список сущностей <see cref="AuctionItemBid" />.</returns>
    Task<IList<AuctionItemBid>> GetListAsync(long auctionItemId);

    /// <summary>
    ///     Получает последнюю (максимальную по цене) ставку для указанного лота аукциона.
    /// </summary>
    /// <param name="auctionItemId">Идентификатор лота, для которого нужно получить последнюю ставку.</param>
    /// <returns>Сущность <see cref="AuctionItemBid" /> — последняя ставка или <c>null</c>, если ставок нет.</returns>
    Task<AuctionItemBid?> GetLastAsync(long auctionItemId);
}