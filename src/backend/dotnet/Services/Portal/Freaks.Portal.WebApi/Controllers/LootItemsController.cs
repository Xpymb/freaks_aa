using Freaks.Portal.Bll.Interfaces.Loot;
using Freaks.Portal.SharedContracts.Dto.Loot;
using Freaks.Users.Attributes;
using Freaks.Users.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freaks.Portal.WebApi.Controllers;

/// <summary>
///     Контроллер для управления предметами добычи (лутом).
///     Предоставляет API для получения списка лута.
/// </summary>
[ApiController]
[Authorize]
[RequireRoles(UserRole.Member)]
[Route("loot-items")]
public class LootItemsController : ControllerBase
{
    private readonly ILootItemService _service;

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="LootItemsController" />.
    /// </summary>
    /// <param name="service">Сервис для работы с предметами добычи.</param>
    public LootItemsController(ILootItemService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    ///     Возвращает список всех предметов добычи.
    /// </summary>
    /// <returns>Список объектов <see cref="LootItemDto" />.</returns>
    [HttpGet]
    public async Task<IList<LootItemDto>> GetListAsync()
    {
        return await _service.GetListAsync();
    }
}