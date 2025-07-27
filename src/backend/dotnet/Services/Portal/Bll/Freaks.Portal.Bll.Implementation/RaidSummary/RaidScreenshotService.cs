using Freaks.Portal.Bll.Interfaces.RaidSummary;
using Freaks.Portal.Contracts.Entities.RaidSummary;
using Freaks.Portal.Dal.Interfaces.RaidSummary;
using Freaks.Portal.SharedContracts.Dto.RaidSummary;
using Freaks.Portal.SharedContracts.Requests.RaidSummary.RaidScreenshot;
using Freaks.Users.Contracts.ValueObjects;
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
    private readonly IRaidScreenshotProvider _provider;

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="RaidScreenshotService" />.
    /// </summary>
    /// <param name="mapper">Маппер для преобразования сущностей в DTO.</param>
    /// <param name="userContext">Контекст текущего пользователя.</param>
    /// <param name="provider">Провайдер доступа к данным скриншотов.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если один из параметров равен null.</exception>
    public RaidScreenshotService(
        IMapper mapper,
        IUserContext userContext,
        IRaidScreenshotProvider provider)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
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
        var entities =
            request.ScreenshotUrls
                   .Select(s =>
                               new RaidScreenshot
                               {
                                   RaidId = raidId,
                                   ScreenshotUrl = s,
                                   CreatorId = _userContext.Id,
                               })
                   .ToList();

        var result = await _provider.SetAsync(entities);

        return _mapper.Map<IList<RaidScreenshotDto>>(result);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(long raidId, string screenshotUrl)
    {
        await _provider.DeleteAsync(new RaidScreenshotKey(raidId, screenshotUrl));
    }
}