using Freaks.Dal.Common.Interfaces;
using Freaks.Dal.Common.ValueObjects;
using Freaks.Users.Bll.Interfaces;
using Freaks.Users.Contracts.Attributes;
using Freaks.Users.Contracts.Extensions;
using Freaks.Users.Contracts.ValueObjects;
using Freaks.Users.Dal.Interfaces;
using Freaks.Users.Dal.Persistence;
using Freaks.Users.SharedContracts.Dto;
using Freaks.Users.SharedContracts.Requests;
using Freaks.WebApi.Common.Exceptions;
using Keycloak.AuthServices.Sdk.Kiota;
using Keycloak.AuthServices.Sdk.Kiota.Admin;
using Keycloak.AuthServices.Sdk.Kiota.Admin.Models;
using MapsterMapper;
using Microsoft.Extensions.Options;

namespace Freaks.Users.Bll.Implementations;

/// <summary>
///     Реализация сервиса пользователей, обеспечивающая доступ к данным через провайдер.
///     Выполняет операции получения, создания и обновления пользователей.
/// </summary>
public class UserService : IUserService
{
    private readonly IUserProvider _provider;
    private readonly IUnitOfWork<UserDbContext> _unitOfWork;
    private readonly IMapper _mapper;
    private readonly KeycloakAdminClientOptions _keycloakOptions;
    private readonly KeycloakAdminApiClient _keycloakClient;

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="UserService" />.
    /// </summary>
    /// <param name="mapper">Маппер.</param>
    /// <param name="keycloakOptions">Keycloak admin настройки</param>
    /// <param name="keycloakClient">Keycloak admin клиент.</param>
    /// <param name="provider">Провайдер пользователей, используемый для доступа к данным.</param>
    /// <param name="unitOfWork">UOW.</param>
    public UserService(
        IMapper mapper,
        IOptions<KeycloakAdminClientOptions> keycloakOptions,
        KeycloakAdminApiClient keycloakClient,
        IUserProvider provider,
        IUnitOfWork<UserDbContext> unitOfWork)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _keycloakOptions = keycloakOptions.Value ?? throw new ArgumentNullException(nameof(keycloakOptions));
        _keycloakClient = keycloakClient ?? throw new ArgumentNullException(nameof(keycloakClient));
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
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

        return await _unitOfWork.ExecuteInsideTransactionAsync(async () =>
        {
            entity.Roles = request.UserRoles;
            entity.UpdatedDt = DateTimeOffset.UtcNow;

            await _provider.UpdateAsync(entity);

            await UpdateKeycloakRolesAsync(userId, request.UserRoles);

            return _mapper.Map<UserDto>(entity);
        });
    }

    /// <summary>
    ///     Обновляет роли пользователя в Keycloak, полностью заменяя текущие назначенные роли на указанные.
    ///     Удаляет все роли, соответствующие значениям перечисления <see cref="UserRole" />, и назначает новые.
    /// </summary>
    /// <param name="userId">Идентификатор пользователя в Keycloak (GUID).</param>
    /// <param name="roles">Список новых ролей, которые необходимо назначить пользователю.</param>
    private async Task UpdateKeycloakRolesAsync(Guid userId, List<UserRole> roles)
    {
        var realmRoles =
            await _keycloakClient.Admin
                                 .Realms[_keycloakOptions.Realm]
                                 .Roles
                                 .GetAsync();

        var allUserRoles =
            Enum.GetValues<UserRole>()
                .ToList();

        await _keycloakClient.Admin
                             .Realms[_keycloakOptions.Realm]
                             .Users[userId.ToString()]
                             .RoleMappings
                             .Realm
                             .DeleteAsync(MapToRoleRepresentations(allUserRoles, realmRoles!));

        var newUserRoles = MapToRoleRepresentations(roles, realmRoles!);
        await _keycloakClient.Admin
                             .Realms[_keycloakOptions.Realm]
                             .Users[userId.ToString()]
                             .RoleMappings
                             .Realm
                             .PostAsync(newUserRoles);
    }

    /// <summary>
    ///     Преобразует список ролей <see cref="UserRole" /> в список <see cref="RoleRepresentation" />,
    ///     используя отображение имён из <see cref="UserRoleNameAttribute" /> и полное описание ролей из Keycloak.
    /// </summary>
    /// <param name="userRoles">Список пользовательских ролей в виде перечисления <see cref="UserRole" />.</param>
    /// <param name="allRealmRoles">Список всех доступных ролей в Realm, полученных из Keycloak.</param>
    /// <returns>Список <see cref="RoleRepresentation" />, соответствующих указанным пользовательским ролям.</returns>
    /// <exception cref="InvalidOperationException">
    ///     Если одна из ролей <see cref="UserRole" /> не сопоставляется ни с одной ролью из <paramref name="allRealmRoles" />.
    /// </exception>
    private static List<RoleRepresentation> MapToRoleRepresentations(List<UserRole> userRoles, List<RoleRepresentation> allRealmRoles)
    {
        var userRoleNames = UserRoleExtensions.GetRoleNames();

        return userRoles.Select(userRole => allRealmRoles.First(r => r.Name == userRoleNames[userRole]))
                        .ToList();
    }
}