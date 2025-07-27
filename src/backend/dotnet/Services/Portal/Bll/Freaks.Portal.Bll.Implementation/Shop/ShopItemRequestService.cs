using Freaks.Dal.Common.Interfaces;
using Freaks.Dal.Common.ValueObjects;
using Freaks.Messages.Bll.Interfaces;
using Freaks.Messages.SharedContracts.Messages.Shop;
using Freaks.Messages.SharedContracts.ValueObjects;
using Freaks.Portal.Bll.Interfaces.Shop;
using Freaks.Portal.Contracts.Entities.Shop;
using Freaks.Portal.Dal.Interfaces.Shop;
using Freaks.Portal.Dal.Persistence;
using Freaks.Portal.SharedContracts.Dto.Shop;
using Freaks.Portal.SharedContracts.Requests.Shop.ShopItemRequest;
using Freaks.Portal.SharedContracts.ValueObjects.Shop;
using Freaks.Users.Contracts.ValueObjects;
using Freaks.WebApi.Common.Exceptions;
using Freaks.WebApi.Common.Exceptions.Shop;
using MapsterMapper;

namespace Freaks.Portal.Bll.Implementation.Shop;

/// <summary>
///     Сервис для управления запросами на покупку товаров в магазине:
///     получение списка запросов, создание, изменение статуса и удаление запросов с публикацией событий.
/// </summary>
public class ShopItemRequestService : IShopItemRequestService
{
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;
    private readonly IShopItemRequestProvider _provider;
    private readonly IShopItemProvider _shopItemProvider;
    private readonly IUnitOfWork<PortalDbContext> _unitOfWork;
    private readonly IMessageService _messageService;

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="ShopItemRequestService" />,
    ///     устанавливая зависимости для работы с запросами и товарами, а также публикации сообщений.
    /// </summary>
    /// <param name="mapper">Маппер для преобразования сущностей в DTO.</param>
    /// <param name="userContext">Контекст текущего пользователя.</param>
    /// <param name="provider">Провайдер для операций над запросами на покупку товаров.</param>
    /// <param name="shopItemProvider">Провайдер для операций над товарами магазина.</param>
    /// <param name="unitOfWork">Юнит работы для обеспечения транзакционной целостности.</param>
    /// <param name="messageService">Сервис для публикации событий об изменениях.</param>
    public ShopItemRequestService(
        IMapper mapper,
        IUserContext userContext,
        IShopItemRequestProvider provider,
        IShopItemProvider shopItemProvider,
        IUnitOfWork<PortalDbContext> unitOfWork,
        IMessageService messageService)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _shopItemProvider = shopItemProvider ?? throw new ArgumentNullException(nameof(shopItemProvider));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
    }

    /// <inheritdoc />
    public async Task<IList<ShopItemRequestDto>> GetListAsync(int shopItemId)
    {
        var result = await _provider.GetListAsync(shopItemId);

        return _mapper.Map<IList<ShopItemRequestDto>>(result);
    }

    /// <inheritdoc />
    public async Task<ShopItemRequestDto> CreateAsync(int shopItemId, CreateShopItemModelRequest request)
    {
        var entity =
            new ShopItemRequest
            {
                ShopItemId = shopItemId,
                UserId = _userContext.Id,
                Quantity = request.Quantity,
                CreatedDt = DateTimeOffset.UtcNow,
                Status = ShopItemRequestStatus.WaitingSubmit,
            };

        var shopItemEntity = await _shopItemProvider.GetAsync(shopItemId, EntityTrackingType.NoTracking);
        if (shopItemEntity is null)
        {
            throw new EntityNotFoundException();
        }

        if (shopItemEntity.Status is ShopItemStatus.Ended)
        {
            throw new ShopItemStatusEndedException();
        }

        if (shopItemEntity.RemainingQuantity - request.Quantity < 0)
        {
            throw new ShopItemDoesNotHaveRequestedQuantity();
        }

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            var result = await _provider.CreateAsync(entity);

            shopItemEntity.RemainingQuantity -= request.Quantity;
            await _shopItemProvider.UpdateAsync(shopItemEntity);

            var message =
                new ShopItemRequestChangedMessage
                {
                    ShopItemId = shopItemId, ActionType = EntityActionType.Created,
                };

            await _messageService.Publish(message);

            return _mapper.Map<ShopItemRequestDto>(result);
        });
    }

    /// <inheritdoc />
    public async Task<ShopItemRequestDto> UpdateStatusAsync(int shopItemId, UpdateStatusShopItemRequest request)
    {
        var entity = await _provider.GetAsync(new ShopItemRequestKey(shopItemId, request.UserId), EntityTrackingType.NoTracking);
        if (entity is null)
        {
            throw new EntityNotFoundException();
        }

        var shopItemEntity = await _shopItemProvider.GetAsync(shopItemId, EntityTrackingType.NoTracking);
        if (shopItemEntity is null)
        {
            throw new EntityNotFoundException();
        }

        return await _unitOfWork.ExecuteAsync(async () =>
        {
            entity.Status = request.Status;
            var result = await _provider.UpdateAsync(entity);

            if (request.Status is ShopItemRequestStatus.Declined)
            {
                shopItemEntity.RemainingQuantity += entity.Quantity;

                await _shopItemProvider.UpdateAsync(shopItemEntity);
            }

            var allRequests = await _provider.GetListAsync(shopItemId);
            if ((shopItemEntity.RemainingQuantity == 0)
                && allRequests.All(r => r.Status != ShopItemRequestStatus.WaitingSubmit))
            {
                shopItemEntity.Status = ShopItemStatus.Ended;

                await _shopItemProvider.UpdateAsync(shopItemEntity);
            }

            var message =
                new ShopItemRequestChangedMessage
                {
                    ShopItemId = shopItemId, ActionType = EntityActionType.Updated,
                };

            await _messageService.Publish(message);

            return _mapper.Map<ShopItemRequestDto>(result);
        });
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int shopItemId, Guid userId)
    {
        var entity = await _provider.GetAsync(new ShopItemRequestKey(shopItemId, userId), EntityTrackingType.NoTracking);
        if (entity is null)
        {
            return;
        }

        if (entity.UserId != _userContext.Id)
        {
            return;
        }

        var shopItemEntity = await _shopItemProvider.GetAsync(shopItemId, EntityTrackingType.NoTracking);
        if (shopItemEntity is null)
        {
            return;
        }

        await _unitOfWork.ExecuteAsync(async () =>
        {
            shopItemEntity.RemainingQuantity += entity.Quantity;
            await _shopItemProvider.UpdateAsync(shopItemEntity);

            await _provider.DeleteAsync(new ShopItemRequestKey(shopItemId, userId));

            var message =
                new ShopItemRequestChangedMessage
                {
                    ShopItemId = shopItemId, ActionType = EntityActionType.Deleted,
                };

            await _messageService.Publish(message);
        });
    }
}