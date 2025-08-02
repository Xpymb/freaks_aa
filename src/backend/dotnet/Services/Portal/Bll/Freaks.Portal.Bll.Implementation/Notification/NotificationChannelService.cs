using Freaks.Portal.Bll.Interfaces.Notification;
using Freaks.Portal.Contracts.Entities.Notification;
using Freaks.Portal.Dal.Interfaces.Notification;
using Freaks.Portal.SharedContracts.Dto.Notification;
using Freaks.Portal.SharedContracts.Requests.Notification.NotificationChannel;
using MapsterMapper;

namespace Freaks.Portal.Bll.Implementation.Notification;
/// <summary>
/// 
/// </summary>
public class NotificationChannelService : INotificationChannelService
{
    
    private readonly INotificationChannelProvider _provider;
    private readonly IMapper _mapper;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="provider"></param>
    /// <param name="mapper"></param>
    /// <exception cref="ArgumentNullException"></exception>
    public NotificationChannelService(INotificationChannelProvider provider, IMapper mapper)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
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
        return _mapper.Map<NotificationChannelDto>(result);
    }
}
