using Freaks.Dal.Common.ValueObjects;
using Freaks.Portal.Bll.Interfaces.RaidSummary;
using Freaks.Portal.Contracts.Entities.RaidSummary;
using Freaks.Portal.Dal.Interfaces.RaidSummary;
using Freaks.Portal.SharedContracts.Dto.RaidSummary;
using Freaks.Portal.SharedContracts.Requests.RaidSummary.RaidLoot;
using Freaks.Users.Bll;
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

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="RaidLootService"/>.
    /// </summary>
    /// <param name="mapper">Сервис AutoMapper для преобразования сущностей в DTO и обратно.</param>
    /// <param name="userContext">Контекст текущего пользователя.</param>
    /// <param name="provider">Провайдер доступа к данным рейдового лута.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если любой из аргументов равен null.</exception>
    public RaidLootService(
        IMapper mapper,
        IUserContext userContext,
        IRaidLootProvider provider)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    /// <inheritdoc />
    public async Task<IList<RaidLootDto>> GetListAsync(int raidId)
    {
        var result = await _provider.GetByRaidIdAsync(raidId);

        return _mapper.Map<IList<RaidLootDto>>(result);
    }

    /// <inheritdoc />
    public async Task<RaidLootDto> CreateAsync(CreateRaidLootRequest request)
    {
        var entity =
            new RaidLoot
            {
                RaidId = request.RaidId,
                LootId = request.LootId,
                Amount = request.Amount,
                CreatorId = _userContext.Id,
            };

        var result = await _provider.CreateAsync(entity);

        return _mapper.Map<RaidLootDto>(result);
    }

    /// <inheritdoc />
    public async Task<RaidLootDto> UpdateAsync(UpdateRaidLootRequest request)
    {
        var entity = await _provider.GetAsync(request.RaidId, request.LootId, EntityTrackingType.NoTracking);
        if (entity is null)
        {
            throw new EntityNotFoundException();
        }

        entity.Amount = request.Amount;

        var result = await _provider.UpdateAsync(entity);

        return _mapper.Map<RaidLootDto>(result);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int raidId, int lootId)
    {
        var entity = await _provider.GetAsync(raidId, lootId, EntityTrackingType.NoTracking);
        if (entity is null)
        {
            return;
        }

        await _provider.DeleteAsync(entity);
    }
}