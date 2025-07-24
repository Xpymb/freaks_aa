using Freaks.Dal.Common.ValueObjects;
using Freaks.Users.Dal;
using Freaks.Users.SharedContracts;
using Freaks.WebApi.Common.Exceptions;
using MapsterMapper;

namespace Freaks.Users.Bll;

/// <summary>
///     Реализация сервиса пользователей, обеспечивающая доступ к данным через провайдер.
///     Выполняет операции получения, создания и обновления пользователей.
/// </summary>
public class UserService : IUserService
{
    private readonly IUserProvider _provider;
    private readonly IMapper _mapper;

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="UserService" />.
    /// </summary>
    /// <param name="provider">Провайдер пользователей, используемый для доступа к данным.</param>
    /// <param name="mapper">Маппер</param>
    public UserService(
        IMapper mapper,
        IUserProvider provider)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    /// <inheritdoc />
    public async Task<UserDto> GetAsync(Guid id)
    {
        var result = await _provider.GetAsync(id, EntityTrackingType.NoTracking);
        if (result is null)
        {
            throw new EntityNotFoundException();
        }

        return _mapper.Map<UserDto>(result);
    }

    /// <inheritdoc />
    public async Task<IList<UserDto>> GetListAsync(bool includeWoRoles)
    {
        var result = await _provider.GetListAsync(includeWoRoles);

        return _mapper.Map<IList<UserDto>>(result);
    }

    /// <inheritdoc />
    public async Task<UserDto> UpdateRolesAsync(Guid userId, UpdateUserRolesRequest request)
    {
        var entity = await _provider.GetAsync(userId, EntityTrackingType.NoTracking);
        if (entity is null)
        {
            throw new EntityNotFoundException();
        }

        entity.Roles = request.UserRoles;
        entity.UpdatedDt = DateTimeOffset.UtcNow;

        await _provider.UpdateAsync(entity);

        return _mapper.Map<UserDto>(entity);
    }
}