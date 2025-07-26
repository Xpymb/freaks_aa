using Freaks.Portal.Bll.Interfaces.RaidSummary;
using Freaks.Portal.SharedContracts.Dto.RaidSummary;
using Freaks.Portal.SharedContracts.Requests.RaidSummary.RaidLoot;
using Freaks.Users.Common.Attributes;
using Freaks.Users.Contracts.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freaks.Portal.WebApi.Controllers.RaidSummary;

/// <summary>
///     Контроллер для управления предметами лута, полученными в рейде.
///     Позволяет просматривать, добавлять, изменять и удалять лут.
/// </summary>
[ApiController]
[Authorize]
[RequireRoles(UserRole.Member)]
[Route("raids/{raidId:int}/loots")]
public class RaidLootController : ControllerBase
{
    private readonly IRaidLootService _service;

    /// <summary>
    ///     Инициализирует новый экземпляр контроллера для управления предметами лута рейда.
    /// </summary>
    /// <param name="service">Сервис для работы с предметами лута рейда.</param>
    public RaidLootController(IRaidLootService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    ///     Получает список предметов лута, связанных с указанным рейдом.
    /// </summary>
    /// <param name="raidId">Идентификатор рейда.</param>
    /// <returns>Список предметов лута рейда.</returns>
    [HttpGet]
    public async Task<IList<RaidLootDto>> GetListAsync([FromRoute] int raidId)
    {
        return await _service.GetListAsync(raidId);
    }

    /// <summary>
    ///     Добавляет новый трофей в указанный рейд.
    /// </summary>
    /// <param name="raidId">Идентификатор рейда.</param>
    /// <param name="request">Данные нового предмета лута.</param>
    /// <returns>Информация о добавленном предмета лута.</returns>
    [HttpPost]
    public async Task<RaidLootDto> CreateAsync([FromRoute] int raidId, [FromBody] CreateRaidLootRequest request)
    {
        return await _service.CreateAsync(raidId, request);
    }

    /// <summary>
    ///     Обновляет информацию о предмета лута в указанном рейде.
    /// </summary>
    /// <param name="raidId">Идентификатор рейда.</param>
    /// <param name="lootId">Идентификатор предмета лута.</param>
    /// <param name="request">Обновлённые данные о предмета лута.</param>
    /// <returns>Информация об обновлённом предмета лута.</returns>
    [HttpPut("{lootId:int}")]
    public async Task<RaidLootDto> UpdateAsync([FromRoute] int raidId, [FromRoute] int lootId, [FromBody] UpdateRaidLootRequest request)
    {
        return await _service.UpdateAsync(raidId, lootId, request);
    }

    /// <summary>
    ///     Удаляет трофей из указанного рейда.
    /// </summary>
    /// <param name="raidId">Идентификатор рейда.</param>
    /// <param name="lootId">Идентификатор предмета лута.</param>
    /// <returns>Результат выполнения операции.</returns>
    [HttpDelete("{lootId:int}")]
    public async Task<ActionResult> DeleteAsync([FromRoute] int raidId, [FromRoute] int lootId)
    {
        await _service.DeleteAsync(raidId, lootId);
        return Ok();
    }
}