using Freaks.Dal.Common.ValueObjects;
using Freaks.Messages.Bll.Interfaces;
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
/// 
/// </summary>
public class NotificationChannelMessageService : INotificationChannelMessageService
{
    private readonly INotificationChannelProvider _channelProvider;
    private readonly INotificationChannelMessageProvider _provider;
    private readonly IMapper _mapper;

    public NotificationChannelMessageService(
        INotificationChannelMessageProvider provider,
        INotificationChannelProvider channelProvider,
        IMapper mapper)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _channelProvider = channelProvider ?? throw new ArgumentNullException(nameof(channelProvider));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<PaginatedList<NotificationChannelMessageDto>> GetListAsync(GetNotificationChannelMessageRequest request)
    {
        var result = await _provider.GetPaginatedListAsync(request);
        return _mapper.Map<PaginatedList<NotificationChannelMessageDto>>(result);
    }
    
    /// <inheritdoc />
    public async Task<NotificationChannelMessageDto> CreateAsync(int notificationChannelId, 
                                                                 CreateNotificationChannelMessageRequest request)
    {
        var channel = await _channelProvider.GetAsync(notificationChannelId, EntityTrackingType.NoTracking);
        if (channel is null)
        {
            throw new EntityNotFoundException();
        }
        
        var entity = new NotificationChannelMessage
        {
            NotificationChannelId = notificationChannelId,
            CreatorId = request.CreatorId,
            Message = request.Message,
            CreatedDt = request.CreatedDt
        };

        var result = await _provider.CreateAsync(entity);
        return _mapper.Map<NotificationChannelMessageDto>(result);
    }
}

