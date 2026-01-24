using Freaks.Dal.Common.ValueObjects;
using Freaks.Messages.Bll.Interfaces;
using Freaks.Messages.SharedContracts.Messages.Shop;
using Freaks.Messages.SharedContracts.ValueObjects;
using Freaks.Portal.Bll.Interfaces.Shop;
using Freaks.Portal.Contracts.Entities.Shop;
using Freaks.Portal.Dal.Interfaces.Shop;
using Freaks.Portal.SharedContracts.Dto.Shop;
using Freaks.Portal.SharedContracts.Requests.Shop.ShopItem;
using Freaks.Portal.SharedContracts.ValueObjects.Shop;
using Freaks.SharedContracts.Common;
using Freaks.Users.Contracts.ValueObjects;
using Freaks.WebApi.Common.Exceptions;
using Freaks.WebApi.Common.Exceptions.Shop;
using MapsterMapper;

namespace Freaks.Portal.Bll.Implementation.Shop;

/// <summary>
///     Сервис для управления товарами в магазине: получение, создание, обновление и удаление позиций с публикацией событий
///     в шину сообщений.
/// </summary>
public class ShopItemService : IShopItemService
{
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;
    private readonly IShopItemProvider _provider;
    private readonly IShopItemRequestProvider _shopItemRequestProvider;
    private readonly IMessageService _messageService;

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="ShopItemService" />,
    ///     устанавливая все необходимые зависимости.
    /// </summary>
    /// <param name="mapper">Маппер для проекции сущностей в DTO.</param>
    /// <param name="userContext">Контекст текущего пользователя.</param>
    /// <param name="provider">Провайдер для операций над товарами магазина.</param>
    /// <param name="shopItemRequestProvider">Провайдер для операций над запросами на покупку товаров.</param>
    /// <param name="messageService">Сервис для публикации событий изменений.</param>
    public ShopItemService(
        IMapper mapper,
        IUserContext userContext,
        IShopItemProvider provider,
        IShopItemRequestProvider shopItemRequestProvider,
        IMessageService messageService)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _shopItemRequestProvider = shopItemRequestProvider ?? throw new ArgumentNullException(nameof(shopItemRequestProvider));
        _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
    }

    /// <inheritdoc />
    public async Task<ShopItemDto> GetAsync(int id)
    {
        var result = await _provider.GetAsync(id, EntityTrackingType.NoTracking);
        if (result is null)
        {
            throw new EntityNotFoundException();
        }

        return _mapper.Map<ShopItemDto>(result);
    }

    /// <inheritdoc />
    public async Task<PaginatedList<ShopItemDto>> GetListAsync(GetShopItemListRequest request)
    {
        var result = await _provider.GetListAsync(request);

        var itemsDto = _mapper.Map<IList<ShopItemDto>>(result.Items);
        return new PaginatedList<ShopItemDto>(itemsDto, result.Take, result.Skip, result.TotalCount);
    }

    /// <inheritdoc />
    public async Task<ShopItemDto> CreateAsync(CreateShopItemRequest request)
    {
        var entity =
            new ShopItem
            {
                LootItemId = request.LootItemId,
                CreatorId = _userContext.Id,
                Price = request.Price,
                TotalQuantity = request.Quantity,
                RemainingQuantity = request.Quantity,
                Status = ShopItemStatus.Active,
            };

        var result = await _provider.CreateAsync(entity);

        await PublishMessageAsync(result.Id, result.Status, EntityActionType.Created);

        return _mapper.Map<ShopItemDto>(result);
    }

    /// <inheritdoc />
    public async Task<ShopItemDto> UpdateAsync(int id, UpdateShopItemRequest request)
    {
        var entity = await _provider.GetAsync(id, EntityTrackingType.NoTracking);
        if (entity is null)
        {
            throw new EntityNotFoundException();
        }

        if (entity.Status is ShopItemStatus.Ended)
        {
            throw new ShopItemStatusEndedException();
        }

        if (entity.TotalQuantity < request.Quantity)
        {
            entity.RemainingQuantity += request.Quantity - entity.TotalQuantity;
        }
        else
        {
            var deltaQuantity = request.Quantity - entity.TotalQuantity;
            if (entity.RemainingQuantity < Math.Abs(deltaQuantity))
            {
                throw new ShopItemDoesNotHaveRequestedQuantity();
            }

            entity.RemainingQuantity += deltaQuantity;
        }

        entity.Price = request.Price;
        entity.TotalQuantity = request.Quantity;

        var requests = await _shopItemRequestProvider.GetListAsync(entity.Id);
        if ((entity.RemainingQuantity == 0)
            && requests.All(r => r.Status != ShopItemRequestStatus.WaitingSubmit))
        {
            entity.Status = ShopItemStatus.Ended;
        }

        var result = await _provider.UpdateAsync(entity);

        await PublishMessageAsync(result.Id, result.Status, EntityActionType.Updated);

        return _mapper.Map<ShopItemDto>(result);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id)
    {
        await _provider.DeleteAsync(id);

        await PublishMessageAsync(id, null, EntityActionType.Deleted);
    }

    private Task PublishMessageAsync(int id, ShopItemStatus? status, EntityActionType actionType)
    {
        var message =
            new ShopItemChangedMessage
            {
                Id = id,
                Status = status,
                ActionType = actionType,
            };

        return _messageService.PublishAsync(message);
    }
}