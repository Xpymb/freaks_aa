using Freaks.Dal.Common.Interfaces;
using Freaks.Portal.Contracts.Entities.Auction;
using Freaks.Portal.Dal.Persistence;
using Freaks.Portal.SharedContracts.Requests.Auction.Item;
using Freaks.SharedContracts.Common;

namespace Freaks.Portal.Dal.Interfaces.Auction;

/// <summary>
///     Провайдер для работы с сущностями лотов аукциона в контексте портального БД.
/// </summary>
public interface IAuctionItemProvider : IBaseProvider<AuctionItem, long, IPortalDbContext>
{
    /// <summary>
    ///     Получает постраничный список лотов аукциона по заданным параметрам.
    /// </summary>
    /// <param name="request">Параметры фильтрации, сортировки и постраничной навигации для выборки лотов.</param>
    /// <returns>Объект <see cref="PaginatedList{AuctionItem}" />, содержащий список лотов и информацию о пагинации.</returns>
    Task<PaginatedList<AuctionItem>> GetListAsync(GetAuctionItemListRequest request);
}