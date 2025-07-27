using Freaks.Portal.SharedContracts.Dto.Auction;
using Freaks.Portal.SharedContracts.Requests.Auction.Bid;

namespace Freaks.Portal.Bll.Interfaces.Auction;

/// <summary>
///     Сервис для работы со ставками по лотам аукциона: получение, создание и удаление ставок.
/// </summary>
public interface IAuctionItemBidService
{
    /// <summary>
    ///     Получает список ставок для указанного лота аукциона.
    /// </summary>
    /// <param name="auctionItemId">Идентификатор лота, для которого запрашиваются ставки.</param>
    /// <returns>Список DTO ставок <see cref="AuctionItemBidDto" />.</returns>
    Task<IList<AuctionItemBidDto>> GetListAsync(long auctionItemId);

    /// <summary>
    ///     Создаёт новую ставку для указанного лота аукциона.
    /// </summary>
    /// <param name="auctionItemId">Идентификатор лота, по которому делается ставка.</param>
    /// <param name="request">Параметры создания ставки (<see cref="CreateAuctionItemBidRequest" />).</param>
    /// <returns>DTO только что созданной ставки <see cref="AuctionItemBidDto" />.</returns>
    Task<AuctionItemBidDto> CreateAsync(long auctionItemId, CreateAuctionItemBidRequest request);

    /// <summary>
    ///     Удаляет ставку по её идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор ставки для удаления.</param>
    Task DeleteAsync(long id);
}