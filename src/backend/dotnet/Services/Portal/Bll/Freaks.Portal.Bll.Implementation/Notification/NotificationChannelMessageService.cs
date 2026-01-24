using Freaks.Dal.Common.ValueObjects;
using Freaks.Messages.Bll.Interfaces;
using Freaks.Messages.SharedContracts.Messages.Notification;
using Freaks.Messages.SharedContracts.ValueObjects;
using Freaks.Portal.Bll.Interfaces.Notification;
using Freaks.Portal.Contracts.Entities.Notification;
using Freaks.Portal.Dal.Interfaces.Notification;
using Freaks.Portal.SharedContracts.Dto.Notification;
using Freaks.Portal.SharedContracts.Requests.Notification.NotificationChannelMessage;
using Freaks.SharedContracts.Common;
using Freaks.WebApi.Common.Exceptions;
using MapsterMapper;

namespace Freaks.Portal.Bll.Implementation.Notification;

/// <summary>
/// Сервис для работы с сообщениями Discord-каналов: получение списка сообщений с пагинацией,
/// создание новых сообщений и публикация соответствующих событий.
/// </summary>
public class NotificationChannelMessageService : INotificationChannelMessageService
{
    private readonly INotificationChannelProvider _channelProvider;
    private readonly INotificationChannelMessageProvider _provider;
    private readonly IMapper _mapper;
    private readonly IMessageService _messageService;
    
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="NotificationChannelMessageService"/>.
    /// </summary>
    /// <param name="provider">Провайдер для работы с сообщениями каналов.</param>
    /// <param name="channelProvider">Провайдер для работы с каналами.</param>
    /// <param name="messageService">Сервис для публикации событий об изменениях сообщений.</param>
    /// <param name="mapper">Маппер для преобразования между DTO и сущностями.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если любой из параметров равен null.</exception>
    public NotificationChannelMessageService(
        INotificationChannelMessageProvider provider,
        INotificationChannelProvider channelProvider,
        IMessageService messageService,
        IMapper mapper)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _channelProvider = channelProvider ?? throw new ArgumentNullException(nameof(channelProvider));
        _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    
    /// <inheritdoc />
    public async Task<PaginatedList<NotificationChannelMessageDto>> GetListAsync(GetNotificationChannelMessageRequest request)
    {
        var result = await _provider.GetPaginatedListAsync(request);
        return _mapper.Map<PaginatedList<NotificationChannelMessageDto>>(result);
    }
    
    /// <inheritdoc />
    public async Task<NotificationChannelMessageDto> CreateAsync(int channelId, 
                                                                 CreateNotificationChannelMessageRequest request)
    {
        var channel = await _channelProvider.GetAsync(channelId, EntityTrackingType.NoTracking);
        if (channel is null)
        {
            throw new EntityNotFoundException();
        }
        
        var entity = new NotificationChannelMessage
        {
            NotificationChannelId = channelId,
            CreatorId = request.CreatorId,
            Message = request.Message,
            CreatedDt = request.CreatedDt
        };

        var result = await _provider.CreateAsync(entity);
        
        await PublishMessageAsync(result.Id, channel.Id, EntityActionType.Created);
        
        return _mapper.Map<NotificationChannelMessageDto>(result);
    }

    private Task PublishMessageAsync(long messageId, long channelId, EntityActionType actionType)
    {
        var message =
            new NotificationChannelMessageChangedMessage
            {
                MessageId = messageId,
                ChannelId = channelId,
                ActionType = actionType
            };

        return _messageService.PublishAsync(message);
    }
}

