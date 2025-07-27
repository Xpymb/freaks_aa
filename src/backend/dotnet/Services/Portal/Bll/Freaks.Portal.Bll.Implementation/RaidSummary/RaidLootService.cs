using Freaks.Dal.Common.ValueObjects;
using Freaks.Messages.Bll.Interfaces;
using Freaks.Messages.SharedContracts.Messages.RaidSummary;
using Freaks.Messages.SharedContracts.ValueObjects;
using Freaks.Portal.Bll.Interfaces.RaidSummary;
using Freaks.Portal.Contracts.Entities.RaidSummary;
using Freaks.Portal.Dal.Interfaces.RaidSummary;
using Freaks.Portal.SharedContracts.Dto.RaidSummary;
using Freaks.Portal.SharedContracts.Requests.RaidSummary.RaidLoot;
using Freaks.Users.Contracts.ValueObjects;
using Freaks.WebApi.Common.Exceptions;
using MapsterMapper;

namespace Freaks.Portal.Bll.Implementation.RaidSummary;

/// <summary>
/// Сервис для управления лутом, полученным в рейдах.
/// Осуществляет бизнес-логику добавления, обновления, удаления и получения предметов лута,
/// а также маппинг сущностей в DTO.
/// </summary>
public class RaidLootService : IRaidLootService
{
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;
    private readonly IRaidLootProvider _provider;
    private readonly IMessageService _messageService;

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="RaidLootService"/>.
    /// </summary>
    /// <param name="mapper">Сервис AutoMapper для преобразования сущностей в DTO и обратно.</param>
    /// <param name="userContext">Контекст текущего пользователя.</param>
    /// <param name="provider">Провайдер доступа к данным рейдового лута.</param>
    /// <param name="messageService">Сервис для публикации сообщений в систему обмена сообщениями.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если любой из аргументов равен null.</exception>
    public RaidLootService(
        IMapper mapper,
        IUserContext userContext,
        IRaidLootProvider provider,
        IMessageService messageService)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
    }

    /// <inheritdoc />
    public async Task<IList<RaidLootDto>> GetListAsync(long raidId)
    {
        var result = await _provider.GetByRaidIdAsync(raidId);

        return _mapper.Map<IList<RaidLootDto>>(result);
    }

    /// <inheritdoc />
    public async Task<RaidLootDto> CreateAsync(long raidId, CreateRaidLootRequest request)
    {
        var entity =
            new RaidLoot
            {
                RaidId = raidId,
                LootItemId = request.LootId,
                Quantity = request.Quantity,
                CreatorId = _userContext.Id,
            };

        var result = await _provider.CreateAsync(entity);

        var message =
            new RaidLootChangedMessage
            {
                RaidId = raidId, ActionType = EntityActionType.Created,
            };

        await _messageService.Publish(message);

        return _mapper.Map<RaidLootDto>(result!);
    }

    /// <inheritdoc />
    public async Task<RaidLootDto> UpdateAsync(long raidId, int lootId, UpdateRaidLootRequest request)
    {
        var entity = await _provider.GetAsync(new RaidLootKey(raidId, lootId), EntityTrackingType.NoTracking);
        if (entity is null)
        {
            throw new EntityNotFoundException();
        }

        entity.Quantity = request.Quantity;

        var result = await _provider.UpdateAsync(entity);

        var message =
            new RaidLootChangedMessage
            {
                RaidId = raidId, ActionType = EntityActionType.Updated,
            };

        await _messageService.Publish(message);

        return _mapper.Map<RaidLootDto>(result);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(long raidId, int lootId)
    {
        await _provider.DeleteAsync(new RaidLootKey(raidId, lootId));

        var message =
            new RaidLootChangedMessage
            {
                RaidId = raidId, ActionType = EntityActionType.Deleted,
            };

        await _messageService.Publish(message);
    }
}