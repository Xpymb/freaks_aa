using Freaks.Portal.Bll.Interfaces.RaidSummary;
using Freaks.Portal.Contracts.Entities.RaidSummary;
using Freaks.Portal.Dal.Interfaces.RaidSummary;
using Freaks.Portal.SharedContracts.Dto.RaidSummary;
using Freaks.Portal.SharedContracts.Requests.RaidSummary.RaidParticipant;
using Freaks.Users.Contracts.ValueObjects;
using MapsterMapper;

namespace Freaks.Portal.Bll.Implementation.RaidSummary;

/// <summary>
///     Сервис для управления участниками рейдов.
///     Отвечает за бизнес-логику добавления, получения и удаления участников.
/// </summary>
public class RaidParticipantService : IRaidParticipantService
{
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;
    private readonly IRaidParticipantProvider _provider;

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="RaidParticipantService" />.
    /// </summary>
    /// <param name="mapper">Маппер для преобразования сущностей в DTO.</param>
    /// <param name="userContext">Контекст текущего пользователя.</param>
    /// <param name="provider">Провайдер доступа к данным участников рейда.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если один из параметров равен null.</exception>
    public RaidParticipantService(
        IMapper mapper,
        IUserContext userContext,
        IRaidParticipantProvider provider)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    /// <inheritdoc />
    public async Task<IList<RaidParticipantDto>> GetListAsync(int raidId)
    {
        var result = await _provider.GetByRaidIdAsync(raidId);

        return _mapper.Map<IList<RaidParticipantDto>>(result);
    }

    /// <inheritdoc />
    public async Task<RaidParticipantDto> CreateAsync(int raidId, CreateRaidParticipantRequest request)
    {
        var entity =
            new RaidParticipant
            {
                RaidId = raidId,
                ParticipantId = request.ParticipantId,
                CreatorId = _userContext.Id,
            };

        var result = await _provider.CreateAsync(entity);

        return _mapper.Map<RaidParticipantDto>(result!);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(int raidId, Guid participantId)
    {
        await _provider.DeleteAsync(new RaidParticipantKey(raidId, participantId));
    }
}