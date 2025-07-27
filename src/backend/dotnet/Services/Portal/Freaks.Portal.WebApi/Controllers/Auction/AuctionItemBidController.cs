using Freaks.Portal.Bll.Interfaces.Auction;
using Freaks.Portal.SharedContracts.Dto.Auction;
using Freaks.Portal.SharedContracts.Requests.Auction.Bid;
using Freaks.Users.Common.Attributes;
using Freaks.Users.Contracts.ValueObjects;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Freaks.Portal.WebApi.Controllers.Auction;

/// <summary>
///     Контроллер для работы со ставками по конкретному лоту аукциона через HTTP API.
/// </summary>
[ApiController]
[Authorize]
[RequireRoles(UserRole.Member)]
[Route("auction/{auctionItemId:long}/bids")]
public class AuctionItemBidController : ControllerBase
{
    private readonly IAuctionItemBidService _service;

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="AuctionItemBidController" />,
    ///     устанавливая сервис для операций со ставками аукциона.
    /// </summary>
    /// <param name="service">Сервис <see cref="IAuctionItemBidService" /> для работы со ставками.</param>
    public AuctionItemBidController(IAuctionItemBidService service)
    {
        _service = service ?? throw new ArgumentNullException(nameof(service));
    }

    /// <summary>
    ///     Получает список ставок для указанного лота аукциона.
    /// </summary>
    /// <param name="auctionItemId">Идентификатор лота, для которого запрашиваются ставки.</param>
    /// <returns>Список DTO ставок <see cref="AuctionItemBidDto" />.</returns>
    [HttpGet]
    public async Task<IList<AuctionItemBidDto>> GetListAsync([FromRoute] long auctionItemId)
    {
        return await _service.GetListAsync(auctionItemId);
    }

    /// <summary>
    ///     Создаёт новую ставку для указанного лота аукциона.
    /// </summary>
    /// <param name="auctionItemId">Идентификатор лота, по которому делается ставка.</param>
    /// <param name="request">Параметры создания ставки (<see cref="CreateAuctionItemBidRequest" />).</param>
    /// <returns>DTO созданной ставки <see cref="AuctionItemBidDto" />.</returns>
    [HttpPost]
    public async Task<AuctionItemBidDto> CreateAsync([FromRoute] long auctionItemId, [FromBody] CreateAuctionItemBidRequest request)
    {
        return await _service.CreateAsync(auctionItemId, request);
    }

    /// <summary>
    ///     Удаляет ставку по её идентификатору.
    /// </summary>
    /// <param name="id">Идентификатор ставки для удаления.</param>
    /// <returns>HTTP 200 OK при успешном удалении.</returns>
    [HttpDelete("{id:long}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] long id)
    {
        await _service.DeleteAsync(id);
        return Ok();
    }
}