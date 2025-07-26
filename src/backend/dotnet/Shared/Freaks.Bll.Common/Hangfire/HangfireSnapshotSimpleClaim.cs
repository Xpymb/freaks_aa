namespace Freaks.Bll.Common.Hangfire;

/// <summary>
///     Клаймы пользователя в снапшоте
/// </summary>
public class HangfireClaimSnapshot
{
    /// <summary>
    ///     Тип клайма
    /// </summary>
    public required string Type { get; init; }

    /// <summary>
    ///     Значение клайма
    /// </summary>
    public required string Value { get; init; }
}