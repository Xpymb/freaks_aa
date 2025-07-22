using Freaks.Dal.Common.ValueObjects;
using Freaks.Portal.Bll.Interfaces.RaidSummary;
using Freaks.Portal.Contracts.Entities.RaidSummary;
using Freaks.Portal.Dal.Interfaces.RaidSummary;
using Freaks.Portal.SharedContracts.Dto.RaidSummary;
using Freaks.Portal.SharedContracts.Requests.RaidSummary.Raid;
using Freaks.SharedContracts.Common;
using Freaks.Users.Bll;
using Freaks.WebApi.Common.Exceptions;
using MapsterMapper;

namespace Freaks.Portal.Bll.Implementation.RaidSummary;

/// <summary>
///     Сервисный класс для управления участниками рейдов.
///     Осуществляет взаимодействие между контроллерами и провайдером данных, включая маппинг и бизнес-логику.
/// </summary>
public class RaidService : IRaidService
{
    private readonly IRaidProvider _provider;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;

    /// <summary>
    ///     Инициализирует новый экземпляр сервиса <see cref="RaidParticipantService" />.
    /// </summary>
    /// <param name="mapper">Сервис AutoMapper для преобразования между моделями.</param>
    /// <param name="userContext">Контекст текущего пользователя.</param>
    /// <param name="provider">Провайдер доступа к данным участников рейда.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если один из аргументов равен null.</exception>
    public RaidService(
        IMapper mapper,
        IUserContext userContext,
        IRaidProvider provider)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
    }

    /// <inheritdoc />
    public async Task<RaidDto> GetAsync(int id)
    {
        var entity = await _provider.GetAsync(id, EntityTrackingType.NoTracking);
        if (entity is null)
        {
            throw new EntityNotFoundException();
        }

        return _mapper.Map<RaidDto>(entity);
    }

    /// <inheritdoc />
    public async Task<PaginatedList<RaidShortDto>> GetListAsync(GetRaidListRequest request)
    {
        var result = await _provider.GetPaginatedListAsync(request);

        return _mapper.Map<PaginatedList<RaidShortDto>>(result);
    }

    /// <inheritdoc />
    public async Task<RaidDto> CreateAsync(CreateRaidRequest request)
    {
        var entity =
            new Raid
            {
                CreatorId = _userContext.Id,
                BossType = request.BossType,
                StartDt = request.StartDt,
                CreatedDt = DateTimeOffset.UtcNow,
                Description = request.Description,
            };

        var result = await _provider.CreateAsync(entity);

        return _mapper.Map<RaidDto>(result);
    }

    /// <inheritdoc />
    public async Task<RaidDto> UpdateAsync(UpdateRaidRequest request)
    {
        var entity = await _provider.GetAsync(request.Id, EntityTrackingType.NoTracking);
        if (entity is null)
        {
            throw new EntityNotFoundException();
        }

        entity.BossType = request.BossType;
        entity.FormatType = request.FormatType;
        entity.Description = request.Description;

        var result = await _provider.UpdateAsync(entity);

        return _mapper.Map<RaidDto>(result);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int id)
    {
        var entity = await _provider.GetAsync(id, EntityTrackingType.NoTracking);
        if (entity is null)
        {
            return;
        }

        await _provider.DeleteAsync(entity);
    }
}