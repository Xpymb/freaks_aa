using Freaks.Portal.Bll.Interfaces.Auction;
using Freaks.Portal.SharedContracts.Dto.Auction;
using Freaks.Portal.SharedContracts.Requests.Auction.Item;
using Freaks.SharedContracts.Common;
using Freaks.Users.Common.Attributes;
using Freaks.Users.Contracts.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freaks.Portal.WebApi.Controllers.Auction;

/// <summary>
///     Контроллер для управления лотами аукциона через HTTP API.
/// </summary>
[ApiController]
[Authorize]
[RequireRoles(UserRole.Member)]
[Route("auction")]
public class AuctionItemController : ControllerBase
{
    private readonly IAuctionItemService _service;

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="AuctionItemController" />,
    ///     устанавливая сервис для работы с лотами аукциона.
    /// </summary>
    /// <param name="service">Сервис <see cref="IAuctionItemService" /> для операций над лотами.</param>
    public AuctionItemController(IAuctionItemService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    ///     Возвращает полную информацию о лоте аукциона по его идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор лота.</param>
    /// <returns>DTO лота <see cref="AuctionItemDto" />.</returns>
    [HttpGet("{id:long}")]
    public Task<AuctionItemDto> GetAsync([FromRoute] long id)
    {
        return _service.GetAsync(id);
    }

    /// <summary>
    ///     Возвращает постраничный список лотов аукциона согласно параметрам запроса.
    /// </summary>
    /// <param name="request">Параметры фильтрации, сортировки и пагинации (<see cref="GetAuctionItemListRequest" />).</param>
    /// <returns>Пагинированный список DTO кратких данных лотов <see cref="PaginatedList{AuctionItemShortDto}" />.</returns>
    [HttpGet]
    public Task<PaginatedList<AuctionItemShortDto>> GetListAsync([FromQuery] GetAuctionItemListRequest request)
    {
        return _service.GetListAsync(request);
    }

    /// <summary>
    ///     Создаёт новый лот аукциона. Доступно только для администраторов и гильд-лидеров.
    /// </summary>
    /// <param name="request">Параметры создания лота (<see cref="CreateAuctionItemRequest" />).</param>
    /// <returns>DTO созданного лота <see cref="AuctionItemDto" />.</returns>
    [RequireRoles(UserRole.Admin, UserRole.GuildLeader)]
    [HttpPost]
    public Task<AuctionItemDto> CreateAsync([FromBody] CreateAuctionItemRequest request)
    {
        return _service.CreateAsync(request);
    }

    /// <summary>
    ///     Обновляет параметры существующего лота аукциона. Доступно только для администраторов и гильд-лидеров.
    /// </summary>
    /// <param name="id">Идентификатор лота для обновления.</param>
    /// <param name="request">Параметры обновления (<see cref="UpdateAuctionItemRequest" />).</param>
    /// <returns>DTO обновлённого лота <see cref="AuctionItemDto" />.</returns>
    [RequireRoles(UserRole.Admin, UserRole.GuildLeader)]
    [HttpPut("{id:long}")]
    public Task<AuctionItemDto> UpdateAsync([FromRoute] long id, [FromBody] UpdateAuctionItemRequest request)
    {
        return _service.UpdateAsync(id, request);
    }

    /// <summary>
    ///     Удаляет лот аукциона по его идентификатору. Доступно только для администраторов и гильд-лидеров.
    /// </summary>
    /// <param name="id">Идентификатор лота для удаления.</param>
    /// <returns>HTTP 200 OK при успешном удалении.</returns>
    [RequireRoles(UserRole.Admin, UserRole.GuildLeader)]
    [HttpDelete("{id:long}")]
    public Task DeleteAsync([FromRoute] long id)
    {
        return _service.DeleteAsync(id);
    }
}