using Freaks.Portal.Bll.Interfaces.Shop;
using Freaks.Portal.SharedContracts.Dto.Shop;
using Freaks.Portal.SharedContracts.Requests.Shop.ShopItemRequest;
using Freaks.Users.Attributes;
using Freaks.Users.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freaks.Portal.WebApi.Controllers.Shop;

/// <summary>
///     Контроллер для управления заявками на покупку товаров из магазина.
///     Предоставляет методы для получения списка заявок, создания, обновления статуса и удаления.
/// </summary>
[ApiController]
[Authorize]
[RequireRoles(UserRole.Member)]
[Route("shop/{shopItemId:int}/requests")]
public class ShopItemRequestController : ControllerBase
{
    private readonly IShopItemRequestService _service;

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="ShopItemRequestController" /> с внедрённым сервисом обработки заявок.
    /// </summary>
    /// <param name="service">Сервис обработки заявок на товары магазина.</param>
    public ShopItemRequestController(IShopItemRequestService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    ///     Получает список заявок на указанный товар магазина.
    /// </summary>
    /// <param name="shopItemId">Идентификатор товара магазина.</param>
    /// <returns>Список заявок на покупку.</returns>
    [HttpGet]
    public async Task<IList<ShopItemRequestDto>> GetListAsync([FromRoute] int shopItemId)
    {
        return await _service.GetListAsync(shopItemId);
    }

    /// <summary>
    ///     Создаёт новую заявку на указанный товар магазина.
    /// </summary>
    /// <param name="shopItemId">Идентификатор товара магазина.</param>
    /// <param name="request">Данные заявки на покупку.</param>
    /// <returns>Созданная заявка.</returns>
    [HttpPost]
    public async Task<ShopItemRequestDto> CreateAsync([FromRoute] int shopItemId, [FromBody] CreateShopItemModelRequest request)
    {
        return await _service.CreateAsync(shopItemId, request);
    }

    /// <summary>
    ///     Обновляет статус заявки на указанный товар магазина.
    /// </summary>
    /// <param name="shopItemId">Идентификатор товара магазина.</param>
    /// <param name="request">Данные с новым статусом заявки.</param>
    /// <returns>Обновлённая заявка.</returns>
    [HttpPatch]
    public async Task<ShopItemRequestDto> UpdateStatusAsync([FromRoute] int shopItemId, [FromBody] UpdateStatusShopItemRequest request)
    {
        return await _service.UpdateStatusAsync(shopItemId, request);
    }

    /// <summary>
    ///     Удаляет заявку пользователя на указанный товар магазина.
    /// </summary>
    /// <param name="shopItemId">Идентификатор товара магазина.</param>
    /// <param name="userId">Идентификатор пользователя, чью заявку нужно удалить.</param>
    [HttpDelete]
    public async Task DeleteAsync([FromRoute] int shopItemId, [FromQuery] Guid userId)
    {
        await _service.DeleteAsync(shopItemId, userId);
    }
}