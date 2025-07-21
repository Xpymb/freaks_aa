using Freaks.Dal.Common.ValueObjects;
using Freaks.Users.Contracts;
using Freaks.Users.Dal;
using Freaks.WebApi.Common.Exceptions;

namespace Freaks.Users.Bll;

/// <summary>
///     Реализация сервиса пользователей, обеспечивающая доступ к данным через провайдер.
///     Выполняет операции получения, создания и обновления пользователей.
/// </summary>
public class UserService : IUserService
{
    private readonly IUserProvider _provider;

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="UserService" />.
    /// </summary>
    /// <param name="provider">Провайдер пользователей, используемый для доступа к данным.</param>
    public UserService(IUserProvider provider)
    {
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
    }

    /// <inheritdoc />
    public async Task<User> GetAsync(Guid id)
    {
        var result = await _provider.GetAsync(id, EntityTrackingType.NoTracking);
        if (result is null)
        {
            throw new EntityNotFoundException();
        }

        return result;
    }

    /// <inheritdoc />
    public async Task<User> CreateAsync(User user)
    {
        return await _provider.CreateAsync(user);
    }

    /// <inheritdoc />
    public async Task<User> UpdateAsync(User user)
    {
        return await _provider.UpdateAsync(user);
    }
}