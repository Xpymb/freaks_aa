using Freaks.Portal.Contracts.Entities.RaidSummary;
using Freaks.SharedContracts.Common;

namespace Freaks.Portal.Bll.Interfaces.RaidSummary;

/// <summary>
///     Сервис проверки прав доступа к рейдам.
/// </summary>
public interface IRaidAccessService
{
    /// <summary>
    ///     Проверяет, имеет ли текущий пользователь право <paramref name="accessType" />
    ///     на рейд с идентификатором <paramref name="raidId" />.
    /// </summary>
    /// <param name="raidId">Идентификатор рейда.</param>
    /// <param name="accessType">Тип запрашиваемого доступа (например, просмотр, изменение, удаление).</param>
    /// <returns>Задача завершается успешно, если доступ разрешён.</returns>
    Task CheckAccessAsync(long raidId, EntityAccessType accessType);

    /// <summary>
    ///     Проверяет, имеет ли текущий пользователь право <paramref name="accessType" />
    ///     на указанный экземпляр <paramref name="raid" />.
    /// </summary>
    /// <param name="raid">Экземпляр рейда, для которого нужно проверить доступ.</param>
    /// <param name="accessType">Тип запрашиваемого доступа (например, просмотр, изменение, удаление).</param>
    void CheckAccess(Raid raid, EntityAccessType accessType);
}