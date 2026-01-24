using Freaks.Dal.Common.ValueObjects;
using Freaks.Messages.Bll.Interfaces;
using Freaks.Messages.SharedContracts.Messages.RaidSummary;
using Freaks.Messages.SharedContracts.ValueObjects;
using Freaks.Portal.Bll.Interfaces.RaidSummary;
using Freaks.Portal.Contracts.Entities.RaidSummary;
using Freaks.Portal.Dal.Interfaces.RaidSummary;
using Freaks.Portal.SharedContracts.Dto.RaidSummary;
using Freaks.Portal.SharedContracts.Requests.RaidSummary.RaidScreenshot;
using Freaks.Portal.SharedContracts.ValueObjects.RaidSummary;
using Freaks.SharedContracts.Common;
using Freaks.Users.Contracts.ValueObjects;
using Freaks.WebApi.Common.Exceptions;
using MapsterMapper;

namespace Freaks.Portal.Bll.Implementation.RaidSummary;

/// <summary>
///     Сервис для управления скриншотами, прикреплёнными к рейдам.
///     Отвечает за получение, добавление и удаление скриншотов, включая маппинг и авторство.
/// </summary>
public class RaidScreenshotService : IRaidScreenshotService
{
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;
    private readonly IRaidAccessService _raidAccessService;
    private readonly IRaidProvider _raidProvider;
    private readonly IRaidScreenshotProvider _provider;
    private readonly IMessageService _messageService;

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="RaidScreenshotService" />.
    /// </summary>
    /// <param name="mapper">Маппер для преобразования сущностей в DTO.</param>
    /// <param name="userContext">Контекст текущего пользователя.</param>
    /// <param name="raidAccessService">Сервис проверки прав на действия с рейдом.</param>
    /// <param name="raidProvider">Провайдер доступа к данным рейдов.</param>
    /// <param name="provider">Провайдер доступа к данным скриншотов.</param>
    /// <param name="messageService">Сервис для публикации сообщений в систему обмена сообщениями.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если один из параметров равен null.</exception>
    public RaidScreenshotService(
        IMapper mapper,
        IUserContext userContext,
        IRaidAccessService raidAccessService,
        IRaidProvider raidProvider,
        IRaidScreenshotProvider provider,
        IMessageService messageService)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
        _raidAccessService = raidAccessService ?? throw new ArgumentNullException(nameof(raidAccessService));
        _raidProvider = raidProvider ?? throw new ArgumentNullException(nameof(raidProvider));
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
    }

    /// <inheritdoc />
    public async Task<IList<RaidScreenshotDto>> GetListAsync(long raidId)
    {
        var result = await _provider.GetByRaidIdAsync(raidId);

        return _mapper.Map<IList<RaidScreenshotDto>>(result);
    }

    /// <inheritdoc />
    public async Task<IList<RaidScreenshotDto>> SetAsync(long raidId, SetRaidScreenshotRequest request)
    {
        var raid = await _raidProvider.GetAsync(raidId, EntityTrackingType.NoTracking);
        if (raid is null)
        {
            throw new EntityNotFoundException();
        }
        _raidAccessService.CheckAccess(raid, EntityAccessType.Update);
        
        var entities =
            request.ScreenshotUris
                   .Select(s =>
                               new RaidScreenshot
                               {
                                   RaidId = raidId,
                                   ScreenshotUri = s,
                                   CreatorId = _userContext.Id,
                               })
                   .ToList();

        await _provider.SetAsync(entities);

        if (entities.Count != 0 && raid.Status is not RaidStatus.Ended)
        {
            raid.Status = RaidStatus.WaitingSubmit;
            raid.UpdatedDt = DateTimeOffset.UtcNow;
            
            await _raidProvider.UpdateAsync(raid);
            
            var raidChangedMessage = new RaidChangedMessage
            {
                Id = raid.Id,
                ActionType = EntityActionType.Updated,
            };
            await _messageService.PublishAsync(raidChangedMessage);
        }
        
        var result = await _provider.GetByRaidIdAsync(raidId);

        await PublishMessageAsync(raidId, EntityActionType.Created);

        return _mapper.Map<IList<RaidScreenshotDto>>(result);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(long raidId, string screenshotUrl)
    {
        var raid = await _raidProvider.GetAsync(raidId, EntityTrackingType.NoTracking);
        if (raid is null)
        {
            throw new EntityNotFoundException();
        }
        _raidAccessService.CheckAccess(raid, EntityAccessType.Update);
        
        await _provider.DeleteAsync(new RaidScreenshotKey(raidId, screenshotUrl));

        var count = await _provider.CountByRaidAsync(raidId);
        if (count == 0)
        {
            raid.Status = RaidStatus.WaitingScreenshot;
            raid.UpdatedDt = DateTimeOffset.UtcNow;
            
            await _raidProvider.UpdateAsync(raid);
            
            var raidChangedMessage = new RaidChangedMessage
            {
                Id = raid.Id,
                ActionType = EntityActionType.Updated,
            };
            await _messageService.PublishAsync(raidChangedMessage);
        }

        await PublishMessageAsync(raidId, EntityActionType.Deleted);
    }

    private Task PublishMessageAsync(long raidId, EntityActionType actionType)
    {
        var message =
            new RaidScreenshotChangedMessage
            {
                RaidId = raidId, ActionType = actionType,
            };

        return _messageService.PublishAsync(message);
    }
}