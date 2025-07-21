namespace Freaks.Portal.SharedContracts.ValueObjects.Raid;

/// <summary>
/// Перечисление форматов рейда.
/// Определяет, является ли рейд PvE или PvP.
/// </summary>
public enum RaidFormatType
{
    /// <summary>
    /// PvE-формат рейда.
    /// </summary>
    Pve = 1,

    /// <summary>
    /// PvP-формат рейда.
    /// </summary>
    Pvp = 2,
}