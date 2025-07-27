using Freaks.Dal.Common.ValueObjects;
using Freaks.Messages.Bll.Interfaces;
using Freaks.Messages.SharedContracts.Messages.Auction;
using Freaks.Messages.SharedContracts.ValueObjects;
using Freaks.Portal.Bll.Interfaces.Auction;
using Freaks.Portal.Contracts.Entities.Auction;
using Freaks.Portal.Dal.Interfaces.Auction;
using Freaks.Portal.SharedContracts.Dto.Auction;
using Freaks.Portal.SharedContracts.Requests.Auction.Bid;
using Freaks.Portal.SharedContracts.ValueObjects.Auction;
using Freaks.Users.Contracts.ValueObjects;
using Freaks.WebApi.Common.Exceptions;
using Freaks.WebApi.Common.Exceptions.Auction;
using MapsterMapper;

namespace Freaks.Portal.Bll.Implementation.Auction;

/// <summary>
///     Сервис для работы со ставками лотов аукциона: получение списка, создание и удаление ставок с публикацией событий.
/// </summary>
public class AuctionItemBidService : IAuctionItemBidService
{
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;
    private readonly IAuctionItemBidProvider _provider;
    private readonly IAuctionItemProvider _itemProvider;
    private readonly IMessageService _messageService;

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="AuctionItemBidService" />,
    ///     устанавливая все необходимые зависимости.
    /// </summary>
    /// <param name="mapper">Маппер для преобразования сущностей в DTO.</param>
    /// <param name="userContext">Контекст текущего пользователя.</param>
    /// <param name="provider">Провайдер для операций над сущностями ставок.</param>
    /// <param name="itemProvider">Провайдер для операций над лотами аукциона.</param>
    /// <param name="messageService">Сервис для публикации событий об изменении ставок.</param>
    public AuctionItemBidService(
        IMapper mapper,
        IUserContext userContext,
        IAuctionItemBidProvider provider,
        IAuctionItemProvider itemProvider,
        IMessageService messageService)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _itemProvider = itemProvider ?? throw new ArgumentNullException(nameof(itemProvider));
        _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
    }

    /// <inheritdoc />
    public async Task<IList<AuctionItemBidDto>> GetListAsync(long auctionItemId)
    {
        var result = await _provider.GetListAsync(auctionItemId);

        return _mapper.Map<IList<AuctionItemBidDto>>(result);
    }

    /// <inheritdoc />
    public async Task<AuctionItemBidDto> CreateAsync(long auctionItemId, CreateAuctionItemBidRequest request)
    {
        var auctionItem = await _itemProvider.GetAsync(auctionItemId, EntityTrackingType.NoTracking);
        if (auctionItem is null)
        {
            throw new EntityNotFoundException();
        }

        if (auctionItem.Status is AuctionItemStatus.Ended)
        {
            throw new AuctionItemAlreadyEndedException();
        }

        var lastBid = await _provider.GetLastAsync(auctionItemId);
        if (lastBid is not null
            && (request.Price < lastBid.Price + auctionItem.MinPriceStep))
        {
            throw new AuctionItemBidPriceShouldBeHigherException();
        }

        var entity =
            new AuctionItemBid
            {
                AuctionItemId = auctionItemId,
                CreatorId = _userContext.Id,
                Price = request.Price,
                CreatedDt = DateTimeOffset.UtcNow,
            };

        var result = await _provider.CreateAsync(entity);

        await PublishMessageAsync(auctionItem.Id, result.Id, EntityActionType.Created);

        return _mapper.Map<AuctionItemBidDto>(result);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(long id)
    {
        var auctionItem = await _itemProvider.GetAsync(id, EntityTrackingType.NoTracking);
        if (auctionItem is null)
        {
            return;
        }

        if (auctionItem.Status is AuctionItemStatus.Ended)
        {
            return;
        }

        var bid = await _provider.GetAsync(id, EntityTrackingType.NoTracking);
        if (bid is null)
        {
            return;
        }

        if (bid.CreatorId != _userContext.Id)
        {
            return;
        }

        await _provider.DeleteAsync(id);

        await PublishMessageAsync(auctionItem.Id, bid.Id, EntityActionType.Deleted);
    }

    private async Task PublishMessageAsync(long auctionItemId, long bidId, EntityActionType actionType)
    {
        var message =
            new AuctionItemBidChangedMessage
            {
                AuctionItemId = auctionItemId,
                BidId = bidId,
                ActionType = actionType,
            };

        await _messageService.Publish(message);
    }
}