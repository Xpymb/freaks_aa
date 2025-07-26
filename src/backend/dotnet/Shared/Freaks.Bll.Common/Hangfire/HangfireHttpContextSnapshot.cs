using Freaks.Users.Contracts.ValueObjects;

namespace Freaks.Bll.Common.Hangfire;

/// <summary>
///     Снапшот http контекста
/// </summary>
public class HangfireHttpContextSnapshot
{
    /// <summary>
    ///     Контекст пользователя
    /// </summary>
    public required UserContext UserContext { get; init; }

    /// <summary>
    ///     Клаймы пользователя
    /// </summary>
    public required List<HangfireClaimSnapshot> Claims { get; init; }
}