using Freaks.Dal.Common.ValueObjects;
using Freaks.Portal.Bll.Interfaces.RaidSummary;
using Freaks.Portal.Contracts.Entities.RaidSummary;
using Freaks.Portal.Dal.Interfaces.RaidSummary;
using Freaks.Portal.SharedContracts.ValueObjects.RaidSummary;
using Freaks.SharedContracts.Common;
using Freaks.Users.Contracts.ValueObjects;
using Freaks.WebApi.Common.Exceptions;
using Freaks.WebApi.Common.Exceptions.Base;

namespace Freaks.Portal.Bll.Implementation.RaidSummary;

/// <summary>
///     Сервис проверки прав доступа к рейдам.
/// </summary>
public class RaidAccessService : IRaidAccessService
{
    private readonly IUserContext _userContext;
    private readonly IRaidProvider _raidProvider;

    /// <summary>
    /// Инициализирует новый экземпляр <see cref="RaidAccessService"/>.
    /// </summary>
    /// <param name="userContext">Контекст текущего пользователя.</param>
    /// <param name="raidProvider">Провайдер для получения данных по рейдам.</param>
    public RaidAccessService(IUserContext userContext, IRaidProvider raidProvider)
    {
        _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
        _raidProvider = raidProvider ?? throw new ArgumentNullException(nameof(raidProvider));
    }

    /// <inheritdoc />
    public async Task CheckAccessAsync(long raidId, EntityAccessType accessType)
    {
        var raid = await _raidProvider.GetAsync(raidId, EntityTrackingType.NoTracking);
        if (raid is null)
        {
            throw new EntityNotFoundException();
        }

        if (!HasAccess(raid, accessType))
        {
            throw new ForbiddenException();
        }
    }

    /// <inheritdoc />
    public void CheckAccess(Raid raid, EntityAccessType accessType)
    {
        if (!HasAccess(raid, accessType))
        {
            throw new ForbiddenException();
        }
    }

    /// <summary>
    /// Определяет, имеет ли текущий пользователь право <paramref name="accessType"/> к указанному <paramref name="raid"/>.
    /// </summary>
    /// <param name="raid">Рейд, для которого проверяются права.</param>
    /// <param name="accessType">Тип доступа: просмотр, создание, изменение, удаление.</param>
    /// <returns><see langword="true"/>, если доступ разрешён; иначе <see langword="false"/>.</returns>
    /// <exception cref="ForbiddenException">
    /// Если у пользователя отсутствует базовая роль <see cref="UserRole.Member"/>.
    /// </exception>
    /// <exception cref="InternalErrorApiException">
    /// Если у рейда не задан создатель (<see cref="Raid.Creator"/> равен <see langword="null"/>).
    /// </exception>
    /// <exception cref="ArgumentOutOfRangeException">
    /// Если <paramref name="accessType"/> имеет неизвестное значение.
    /// </exception>
    private bool HasAccess(Raid raid, EntityAccessType accessType)
    {
        if (!_userContext.Roles.Contains(UserRole.Member))
        {
            throw new ForbiddenException();
        }

        var isSuperUser = _userContext.Roles.Contains(UserRole.Admin) || _userContext.Roles.Contains(UserRole.GuildLeader);
        var isEditor = _userContext.Roles.Contains(UserRole.Editor);
        
        if (raid.Creator is null)
        {
            throw new InternalErrorApiException();
        }

        return accessType switch
        {
            EntityAccessType.View => true,
            EntityAccessType.Update => (isSuperUser || isEditor || raid.Creator.Id == _userContext.Id) 
                                       && raid.Status is not RaidStatus.Ended,
            EntityAccessType.Create => true,
            EntityAccessType.Delete => isSuperUser 
                                       || (raid.Creator.Id == _userContext.Id && raid.Status is not RaidStatus.Ended),
            _ => throw new ArgumentOutOfRangeException(nameof(accessType), accessType, null)
        };
    }
}