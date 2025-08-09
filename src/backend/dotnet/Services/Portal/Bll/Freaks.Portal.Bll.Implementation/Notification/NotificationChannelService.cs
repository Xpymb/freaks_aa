using Freaks.Messages.Bll.Interfaces;
using Freaks.Messages.SharedContracts.Messages.Notification;
using Freaks.Messages.SharedContracts.ValueObjects;
using Freaks.Portal.Bll.Interfaces.Notification;
using Freaks.Portal.Contracts.Entities.Notification;
using Freaks.Portal.Dal.Interfaces.Notification;
using Freaks.Portal.SharedContracts.Dto.Notification;
using Freaks.Portal.SharedContracts.Requests.Notification.NotificationChannel;
using MapsterMapper;

namespace Freaks.Portal.Bll.Implementation.Notification;
/// <summary>
/// Сервис для работы с Discord-каналами: получение списка каналов,
/// создание новых каналов и публикация соответствующих событий.
/// </summary>
public class NotificationChannelService : INotificationChannelService
{
    private readonly INotificationChannelProvider _provider;
    private readonly IMessageService _messageService;
    private readonly IMapper _mapper;
    
    /// <summary>
    /// Инициализирует новый экземпляр <see cref="NotificationChannelService"/>.
    /// </summary>
    /// <param name="provider">Провайдер для работы с каналами.</param>
    /// <param name="mapper">Маппер для преобразования между DTO и сущностями.</param>
    /// <param name="messageService">Сервис для публикации событий об изменениях каналов.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если любой из параметров равен null.</exception>
    public NotificationChannelService(INotificationChannelProvider provider, IMapper mapper, IMessageService messageService)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
    }
    
    /// <inheritdoc />
    public async Task<IList<NotificationChannelDto>> GetListAsync()
    {
        var result = await _provider.GetListAsync();
        return _mapper.Map<IList<NotificationChannelDto>>(result);
    }
    
    /// <inheritdoc />
    public async Task<NotificationChannelDto> CreateAsync(CreateNotificationChannelRequest request)
    {
        var entity = new NotificationChannel
        {
            DiscordChannelId = request.DiscordChannelId,
            Name = request.Name
        };

        var result = await _provider.CreateAsync(entity);
        
        await PublishMessageAsync(result.Id, EntityActionType.Created);
        
        return _mapper.Map<NotificationChannelDto>(result);
    }
    
    private async Task PublishMessageAsync(long id, EntityActionType actionType)
    {
        var message =
            new NotificationChannelChangedMessage
            {
                Id = id,
                ActionType = actionType
            };

        await _messageService.Publish(message);
    }
}
