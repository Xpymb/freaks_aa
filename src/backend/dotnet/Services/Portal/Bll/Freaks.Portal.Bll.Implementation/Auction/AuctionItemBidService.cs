using Freaks.Dal.Common.ValueObjects;
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

public class AuctionItemBidService : IAuctionItemBidService
{
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;
    private readonly IAuctionItemBidProvider _provider;
    private readonly IAuctionItemProvider _itemProvider;

    public AuctionItemBidService(
        IMapper mapper,
        IUserContext userContext,
        IAuctionItemBidProvider provider,
        IAuctionItemProvider itemProvider)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _itemProvider = itemProvider ?? throw new ArgumentNullException(nameof(itemProvider));
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
    }
}