using Freaks.Messages.Bll.Interfaces;
using Freaks.Messages.SharedContracts.Messages.RaidSummary;
using Freaks.Messages.SharedContracts.ValueObjects;
using Freaks.Portal.Bll.Interfaces.RaidSummary;
using Freaks.Portal.Contracts.Entities.RaidSummary;
using Freaks.Portal.Dal.Interfaces.RaidSummary;
using Freaks.Portal.SharedContracts.Dto.RaidSummary;
using Freaks.Portal.SharedContracts.Requests.RaidSummary.RaidParticipant;
using Freaks.SharedContracts.Common;
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
    private readonly IRaidAccessService _raidAccessService;
    private readonly IRaidParticipantProvider _provider;
    private readonly IMessageService _messageService;

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="RaidParticipantService" />.
    /// </summary>
    /// <param name="mapper">Маппер для преобразования сущностей в DTO.</param>
    /// <param name="userContext">Контекст текущего пользователя.</param>
    /// <param name="raidAccessService">Сервис проверки прав на действия с рейдом.</param>
    /// <param name="provider">Провайдер доступа к данным участников рейда.</param>
    /// <param name="messageService">Сервис для публикации сообщений в систему обмена сообщениями.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если один из параметров равен null.</exception>
    public RaidParticipantService(
        IMapper mapper,
        IUserContext userContext,
        IRaidAccessService raidAccessService,
        IRaidParticipantProvider provider,
        IMessageService messageService)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
        _raidAccessService = raidAccessService ?? throw new ArgumentNullException(nameof(raidAccessService));
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
    }

    /// <inheritdoc />
    public async Task<IList<RaidParticipantDto>> GetListAsync(long raidId)
    {
        var result = await _provider.GetByRaidIdAsync(raidId);

        return _mapper.Map<IList<RaidParticipantDto>>(result);
    }

    /// <inheritdoc />
    public async Task<RaidParticipantDto> CreateAsync(long raidId, CreateRaidParticipantRequest request)
    {
        await _raidAccessService.CheckAccessAsync(raidId, EntityAccessType.Update);
        
        var entity =
            new RaidParticipant
            {
                RaidId = raidId,
                ParticipantId = request.ParticipantId,
                CreatorId = _userContext.Id,
                RaidNumber = request.RaidNumber,
                RaidPartyNumber = request.RaidPartyNumber,
                RaidPartyPositionNumber = request.RaidPartyPositionNumber,
            };

        var result = await _provider.CreateAsync(entity);

        await PublishMessageAsync(raidId, EntityActionType.Created);

        return _mapper.Map<RaidParticipantDto>(result);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(long raidId, Guid participantId)
    {
        await _raidAccessService.CheckAccessAsync(raidId, EntityAccessType.Update);
        
        await _provider.DeleteAsync(new RaidParticipantKey(raidId, participantId));

        await PublishMessageAsync(raidId, EntityActionType.Deleted);
    }

    private async Task PublishMessageAsync(long raidId, EntityActionType actionType)
    {
        var message =
            new RaidParticipantChangedMessage
            {
                RaidId = raidId, ActionType = actionType,
            };

        await _messageService.Publish(message);
    }
}