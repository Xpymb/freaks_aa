using Freaks.Portal.SharedContracts.Dto.Auction;
using Freaks.Portal.SharedContracts.Requests.Auction.Item;
using Freaks.SharedContracts.Common;

namespace Freaks.Portal.Bll.Interfaces.Auction;

/// <summary>
///     Сервис для управления лотами аукциона: получение, создание, обновление и удаление лотов.
/// </summary>
public interface IAuctionItemService
{
    /// <summary>
    ///     Получает полную информацию о лоте по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор лота.</param>
    /// <returns><see cref="AuctionItemDto" /> с полной информацией о лоте.</returns>
    Task<AuctionItemDto> GetAsync(long id);

    /// <summary>
    ///     Получает постраничный список лотов аукциона согласно заданным параметрам.
    /// </summary>
    /// <param name="request">Параметры фильтрации, сортировки и пагинации.</param>
    /// <returns><see cref="PaginatedList{AuctionItemShortDto}" /> с краткими данными по лотам и метаданными пагинации.</returns>
    Task<PaginatedList<AuctionItemShortDto>> GetListAsync(GetAuctionItemListRequest request);

    /// <summary>
    ///     Создаёт новый лот аукциона по переданным параметрам.
    /// </summary>
    /// <param name="request">Параметры нового лота (<see cref="CreateAuctionItemRequest" />).</param>
    /// <returns><see cref="AuctionItemDto" /> с данными только что созданного лота.</returns>
    Task<AuctionItemDto> CreateAsync(CreateAuctionItemRequest request);

    /// <summary>
    ///     Обновляет параметры существующего лота аукциона.
    /// </summary>
    /// <param name="id">Идентификатор лота.</param>
    /// <param name="request">Параметры обновления лота (<see cref="UpdateAuctionItemRequest" />).</param>
    /// <returns><see cref="AuctionItemDto" /> с данными лота после обновления.</returns>
    Task<AuctionItemDto> UpdateAsync(long id, UpdateAuctionItemRequest request);

    /// <summary>
    ///     Удаляет лот аукциона по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор лота для удаления.</param>
    Task DeleteAsync(long id);
}