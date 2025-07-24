using Freaks.Portal.Bll.Interfaces.Shop;
using Freaks.Portal.SharedContracts.Dto.Shop;
using Freaks.Portal.SharedContracts.Requests.Shop.ShopItem;
using Freaks.SharedContracts.Common;
using Freaks.Users.Attributes;
using Freaks.Users.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freaks.Portal.WebApi.Controllers.Shop;

/// <summary>
///     Контроллер для управления товарами в магазине.
///     Доступен только авторизованным пользователям с ролью <see cref="UserRole.Member" />.
///     Предоставляет методы для получения, создания, обновления и удаления товаров,
///     а также для получения списка с поддержкой фильтрации и пагинации.
/// </summary>
[ApiController]
[Authorize]
[RequireRoles(UserRole.Member)]
[Route("shop")]
public class ShopItemController : ControllerBase
{
    private readonly IShopItemService _service;

    /// <summary>
    ///     Инициализирует новый экземпляр контроллера <see cref="ShopItemController" />.
    /// </summary>
    /// <param name="service">Сервис для работы с товарами магазина.</param>
    public ShopItemController(IShopItemService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    ///     Возвращает товар магазина по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор товара.</param>
    /// <returns>Объект <see cref="ShopItemDto" /> с информацией о товаре.</returns>
    [HttpGet("{id:int}")]
    public async Task<ShopItemDto> GetAsync([FromRoute] int id)
    {
        return await _service.GetAsync(id);
    }

    /// <summary>
    ///     Возвращает список товаров с поддержкой фильтрации и постраничного вывода.
    /// </summary>
    /// <param name="request">Параметры фильтрации и пагинации.</param>
    /// <returns>Пагинированный список объектов <see cref="ShopItemDto" />.</returns>
    [HttpGet]
    public async Task<PaginatedList<ShopItemDto>> GetListAsync([FromQuery] GetShopItemListRequest request)
    {
        return await _service.GetListAsync(request);
    }

    /// <summary>
    ///     Создаёт новый товар в магазине.
    /// </summary>
    /// <param name="request">Данные для создания товара.</param>
    /// <returns>Созданный объект <see cref="ShopItemDto" />.</returns>
    [HttpPost]
    public async Task<ShopItemDto> CreateAsync([FromBody] CreateShopItemRequest request)
    {
        return await _service.CreateAsync(request);
    }

    /// <summary>
    ///     Обновляет существующий товар в магазине.
    /// </summary>
    /// <param name="id">Идентификатор товара.</param>
    /// <param name="request">Данные для обновления товара.</param>
    /// <returns>Обновлённый объект <see cref="ShopItemDto" />.</returns>
    [HttpPut("{id:int}")]
    public async Task<ShopItemDto> UpdateAsync([FromRoute] int id, UpdateShopItemRequest request)
    {
        return await _service.UpdateAsync(id, request);
    }

    /// <summary>
    ///     Удаляет товар из магазина по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор удаляемого товара.</param>
    [HttpDelete("{id:int}")]
    public async Task DeleteAsync([FromRoute] int id)
    {
        await _service.DeleteAsync(id);
    }
}