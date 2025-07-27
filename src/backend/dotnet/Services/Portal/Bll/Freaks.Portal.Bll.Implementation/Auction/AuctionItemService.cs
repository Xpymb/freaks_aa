using Freaks.Dal.Common.ValueObjects;
using Freaks.Messages.Bll.Interfaces;
using Freaks.Messages.SharedContracts.Messages.Auction;
using Freaks.Messages.SharedContracts.ValueObjects;
using Freaks.Portal.Bll.Interfaces.Auction;
using Freaks.Portal.Contracts.Entities.Auction;
using Freaks.Portal.Dal.Interfaces.Auction;
using Freaks.Portal.SharedContracts.Dto.Auction;
using Freaks.Portal.SharedContracts.Requests.Auction.Item;
using Freaks.Portal.SharedContracts.ValueObjects.Auction;
using Freaks.SharedContracts.Common;
using Freaks.Users.Contracts.ValueObjects;
using Freaks.WebApi.Common.Exceptions;
using Hangfire;
using MapsterMapper;

namespace Freaks.Portal.Bll.Implementation.Auction;

/// <summary>
///     Сервис для управления лотами аукциона: получение, создание, обновление и удаление лотов.
/// </summary>
public class AuctionItemService : IAuctionItemService
{
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;
    private readonly IBackgroundJobClient _jobClient;
    private readonly IAuctionItemProvider _provider;
    private readonly IAuctionItemBidProvider _bidProvider;
    private readonly IMessageService _messageService;

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="AuctionItemService" />,
    ///     устанавливая необходимые зависимости.
    /// </summary>
    /// <param name="mapper">Автоматический маппер для проекции сущностей в DTO.</param>
    /// <param name="userContext">Контекст текущего пользователя.</param>
    /// <param name="jobClient">Клиент фоновых задач для планирования уведомлений и прочего.</param>
    /// <param name="provider">Провайдер для работы с сущностями лотов аукциона.</param>
    /// <param name="bidProvider">Провайдер для работы со ставками по лотам.</param>
    /// <param name="messageService">Сервис для публикации сообщений в систему обмена сообщениями.</param>
    public AuctionItemService(
        IMapper mapper,
        IUserContext userContext,
        IBackgroundJobClient jobClient,
        IAuctionItemProvider provider,
        IAuctionItemBidProvider bidProvider,
        IMessageService messageService)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
        _jobClient = jobClient ?? throw new ArgumentNullException(nameof(jobClient));
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _bidProvider = bidProvider ?? throw new ArgumentNullException(nameof(bidProvider));
        _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
    }

    /// <inheritdoc />
    public async Task<AuctionItemDto> GetAsync(long id)
    {
        var result = await _provider.GetAsync(id, EntityTrackingType.NoTracking);
        if (result is null)
        {
            throw new EntityNotFoundException();
        }

        var mappedResult = _mapper.Map<AuctionItemDto>(result);

        var lastBid = await _bidProvider.GetLastAsync(id);
        if (lastBid is not null)
        {
            var mappedLastBid = _mapper.Map<AuctionItemBidDto>(lastBid);

            mappedResult.LastBid = mappedLastBid;
        }

        return mappedResult;
    }

    /// <inheritdoc />
    public async Task<PaginatedList<AuctionItemShortDto>> GetListAsync(GetAuctionItemListRequest request)
    {
        var result = await _provider.GetListAsync(request);

        var mappedResult = _mapper.Map<PaginatedList<AuctionItemShortDto>>(result);

        foreach (var item in mappedResult.Items)
        {
            var lastBid = await _bidProvider.GetLastAsync(item.Id);
            if (lastBid is not null)
            {
                item.LastBidPrice = lastBid.Price;
            }
        }

        return mappedResult;
    }

    /// <inheritdoc />
    public async Task<AuctionItemDto> CreateAsync(CreateAuctionItemRequest request)
    {
        var entity =
            new AuctionItem
            {
                LootItemId = request.LootItemId,
                StartPrice = request.StartPrice,
                MinPriceStep = request.MinPriceStep,
                CreatedDt = DateTimeOffset.UtcNow,
                EndDt = request.EndDt,
                Status = AuctionItemStatus.Started,
                CreatorId = _userContext.Id,
                Description = request.Description,
            };

        var result = await _provider.CreateAsync(entity);

        _jobClient.Schedule<AuctionItemService>(
            svc => svc.FinishAuctionItemAsync(result.Id),
            request.EndDt
        );

        var message =
            new AuctionItemChangedMessage
            {
                Id = result.Id,
                Status = result.Status,
                ActionType = EntityActionType.Created,
            };

        await _messageService.Publish(message);

        return _mapper.Map<AuctionItemDto>(result);
    }

    /// <inheritdoc />
    public async Task<AuctionItemDto> UpdateAsync(long id, UpdateAuctionItemRequest request)
    {
        var entity = await _provider.GetAsync(id, EntityTrackingType.NoTracking);
        if (entity is null)
        {
            throw new EntityNotFoundException();
        }

        entity.MinPriceStep = request.MinPriceStep;
        entity.Description = request.Description;

        var result = await _provider.UpdateAsync(entity);

        var message =
            new AuctionItemChangedMessage
            {
                Id = result.Id,
                Status = result.Status,
                ActionType = EntityActionType.Updated,
            };

        await _messageService.Publish(message);

        return await GetAsync(result.Id);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(long id)
    {
        await _provider.DeleteAsync(id);

        var message =
            new AuctionItemChangedMessage
            {
                Id = id, ActionType = EntityActionType.Deleted,
            };

        await _messageService.Publish(message);
    }

    /// <summary>
    ///     Завершает аукцион для указанного лота, устанавливая ему статус <see cref="AuctionItemStatus.Ended" />.
    /// </summary>
    /// <param name="id">Идентификатор аукционного лота, который необходимо завершить.</param>
    /// <exception cref="EntityNotFoundException">
    ///     Бросается, если лот с указанным идентификатором не найден в базе.
    /// </exception>
    [AutomaticRetry(Attempts = 2, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
    // ReSharper disable once MemberCanBePrivate.Global
    public async Task FinishAuctionItemAsync(long id)
    {
        var entity = await _provider.GetAsync(id, EntityTrackingType.NoTracking);
        if (entity is null)
        {
            throw new EntityNotFoundException();
        }

        entity.Status = AuctionItemStatus.Ended;

        await _provider.UpdateAsync(entity);

        var message =
            new AuctionItemChangedMessage
            {
                Id = entity.Id,
                Status = entity.Status,
                ActionType = EntityActionType.Updated,
            };

        await _messageService.Publish(message);
    }
}