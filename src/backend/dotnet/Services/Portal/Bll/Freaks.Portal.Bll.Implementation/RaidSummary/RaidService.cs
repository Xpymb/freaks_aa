using Freaks.Dal.Common.ValueObjects;
using Freaks.Messages.Bll.Interfaces;
using Freaks.Messages.SharedContracts.Messages.RaidSummary;
using Freaks.Messages.SharedContracts.ValueObjects;
using Freaks.Portal.Bll.Interfaces.RaidSummary;
using Freaks.Portal.Contracts.Entities.RaidSummary;
using Freaks.Portal.Dal.Interfaces.RaidSummary;
using Freaks.Portal.SharedContracts.Dto.RaidSummary;
using Freaks.Portal.SharedContracts.Requests.RaidSummary.Raid;
using Freaks.Portal.SharedContracts.ValueObjects.RaidSummary;
using Freaks.SharedContracts.Common;
using Freaks.Users.Contracts.ValueObjects;
using Freaks.WebApi.Common.Exceptions;
using Hangfire;
using MapsterMapper;

namespace Freaks.Portal.Bll.Implementation.RaidSummary;

/// <summary>
///     Сервисный класс для управления участниками рейдов.
///     Осуществляет взаимодействие между контроллерами и провайдером данных, включая маппинг и бизнес-логику.
/// </summary>
public class RaidService : IRaidService
{
    private readonly IRaidProvider _provider;
    private readonly IBackgroundJobClient _jobClient;
    private readonly IMessageService _messageService;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;

    /// <summary>
    ///     Инициализирует новый экземпляр сервиса <see cref="RaidParticipantService" />.
    /// </summary>
    /// <param name="mapper">Сервис AutoMapper для преобразования между моделями.</param>
    /// <param name="userContext">Контекст текущего пользователя.</param>
    /// <param name="provider">Провайдер доступа к данным участников рейда.</param>
    /// <param name="jobClient">Клиент фоновых задач.</param>
    /// <param name="messageService">Сервис для публикации сообщений в систему обмена сообщениями.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если один из аргументов равен null.</exception>
    public RaidService(
        IMapper mapper,
        IUserContext userContext,
        IRaidProvider provider,
        IBackgroundJobClient jobClient,
        IMessageService messageService)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _jobClient = jobClient ?? throw new ArgumentNullException(nameof(jobClient));
        _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
    }

    /// <inheritdoc />
    public async Task<RaidDto> GetAsync(long id)
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
        var status = RaidStatus.Planned;
        if (DateTimeOffset.UtcNow > request.StartDt)
        {
            status = RaidStatus.WaitingScreenshot;
        }
        
        var entity =
            new Raid
            {
                CreatorId = _userContext.Id,
                BossType = request.BossType,
                StartDt = request.StartDt,
                Status = status,
                CreatedDt = DateTimeOffset.UtcNow,
                Description = request.Description,
            };

        var result = await _provider.CreateAsync(entity);

        if (status is RaidStatus.Planned)
        {
            _jobClient.Schedule<RaidService>(
                svc => svc.EvaluateRaidStartAsync(result.Id),
                request.StartDt
            );
        }

        await PublishMessageAsync(result.Id, EntityActionType.Created);

        return _mapper.Map<RaidDto>(result);
    }

    /// <inheritdoc />
    public async Task<RaidDto> UpdateAsync(long id, UpdateRaidRequest request)
    {
        var entity = await _provider.GetAsync(id, EntityTrackingType.NoTracking);
        if (entity is null)
        {
            throw new EntityNotFoundException();
        }

        entity.BossType = request.BossType;
        entity.FormatType = request.FormatType;
        entity.Description = request.Description;

        var result = await _provider.UpdateAsync(entity);

        await PublishMessageAsync(result.Id, EntityActionType.Updated);

        return _mapper.Map<RaidDto>(result);
    }

    /// <inheritdoc />
    public async Task<RaidDto> FinishAsync(long id)
    {
        var entity = await _provider.GetAsync(id, EntityTrackingType.NoTracking);
        if (entity is null)
        {
            throw new EntityNotFoundException();
        }

        entity.Status = RaidStatus.Ended;
        
        var result = await _provider.UpdateAsync(entity);
        
        await PublishMessageAsync(result.Id, EntityActionType.Updated);

        return _mapper.Map<RaidDto>(result);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(long id)
    {
        await _provider.DeleteAsync(id);

        await PublishMessageAsync(id, EntityActionType.Deleted);
    }

    /// <summary>
    ///     Проверяет, находится ли рейд в статусе <see cref="RaidStatus.Planned" />, и если да — переводит его в статус
    ///     <see cref="RaidStatus.WaitingScreenshot" />.
    ///     Метод используется как отложенная задача (например, через Hangfire) для автоматического обновления статуса рейда
    ///     в момент времени начала.
    ///     В случае отсутствия рейда выбрасывает <see cref="EntityNotFoundException" />.
    ///     Повторяется до 2 раз при ошибках, после чего удаляется из очереди.
    /// </summary>
    /// <param name="id">Идентификатор рейда</param>
    /// <exception cref="EntityNotFoundException">Если рейд с указанным идентификатором не найден</exception>
    [AutomaticRetry(Attempts = 2, OnAttemptsExceeded = AttemptsExceededAction.Delete)]
    // ReSharper disable once MemberCanBePrivate.Global
    public async Task EvaluateRaidStartAsync(long id)
    {
        var entity = await _provider.GetAsync(id, EntityTrackingType.NoTracking);
        if (entity is null)
        {
            throw new EntityNotFoundException();
        }

        if (entity.Status is not RaidStatus.Planned)
        {
            return;
        }

        entity.Status = RaidStatus.WaitingScreenshot;
        entity.UpdatedDt = DateTimeOffset.UtcNow;

        await _provider.UpdateAsync(entity);

        await PublishMessageAsync(entity.Id, EntityActionType.Updated);
    }

    private async Task PublishMessageAsync(long id, EntityActionType actionType)
    {
        var message =
            new RaidChangedMessage
            {
                Id = id, ActionType = actionType,
            };

        await _messageService.Publish(message);
    }
}