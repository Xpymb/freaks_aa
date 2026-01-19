using Freaks.Dal.Common.ValueObjects;
using Freaks.Messages.Bll.Interfaces;
using Freaks.Messages.SharedContracts.Messages.SalarySummary;
using Freaks.Messages.SharedContracts.ValueObjects;
using Freaks.Portal.Bll.Interfaces.SalarySummary;
using Freaks.Portal.Contracts.Entities.SalarySummary;
using Freaks.Portal.Dal.Interfaces.SalarySummary;
using Freaks.Portal.SharedContracts.Dto.SalarySummary;
using Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryLoot;
using Freaks.WebApi.Common.Exceptions;
using MapsterMapper;

namespace Freaks.Portal.Bll.Implementation.SalarySummary;

/// <summary>
///     Сервис для управления проданным лутом за зарплатные периоды.
///     Осуществляет бизнес-логику добавления, обновления, удаления и получения записей о проданном луте,
///     а также маппинг сущностей в DTO. Включает финансовую логику с обратным пересчётом.
/// </summary>
public class SalaryLootService : ISalaryLootService
{
    private readonly IMapper _mapper;
    private readonly ISalaryLootProvider _provider;
    private readonly IMessageService _messageService;

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="SalaryLootService"/>.
    /// </summary>
    /// <param name="mapper">Сервис Mapster для преобразования сущностей в DTO и обратно.</param>
    /// <param name="provider">Провайдер доступа к данным проданного лута зарплатных периодов.</param>
    /// <param name="messageService">Сервис для публикации сообщений в систему обмена сообщениями.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если любой из аргументов равен null.</exception>
    public SalaryLootService(
        IMapper mapper,
        ISalaryLootProvider provider,
        IMessageService messageService)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
    }

    /// <inheritdoc />
    public async Task<IList<SalaryLootDto>> GetListAsync(long salaryId)
    {
        var result = await _provider.GetBySalaryIdAsync(salaryId);

        return _mapper.Map<IList<SalaryLootDto>>(result);
    }

    /// <inheritdoc />
    public async Task<SalaryLootDto> CreateAsync(long salaryId, CreateSalaryLootRequest request)
    {
        var amount = request.Quantity * request.PricePerItem * (1 - request.DiscountPercent / 100);

        var entity =
            new SalaryLoot
            {
                SalaryId = salaryId,
                LootId = request.LootId,
                Quantity = request.Quantity,
                PricePerItem = request.PricePerItem,
                DiscountPercent = request.DiscountPercent,
                Amount = amount,
            };

        var result = await _provider.CreateAsync(entity);

        await PublishMessageAsync(salaryId, EntityActionType.Created);

        return _mapper.Map<SalaryLootDto>(result!);
    }

    /// <inheritdoc />
    public async Task<SalaryLootDto> UpdateAsync(long salaryId, long lootId, UpdateSalaryLootRequest request)
    {
        var entity = await _provider.GetAsync(lootId, EntityTrackingType.NoTracking);
        if (entity is null)
        {
            throw new EntityNotFoundException();
        }

        entity.LootId = request.LootId;
        entity.Quantity = request.Quantity;
        entity.PricePerItem = request.PricePerItem;

        if (request.DiscountPercent.HasValue)
        {
            entity.DiscountPercent = request.DiscountPercent.Value;
            entity.Amount = entity.Quantity * entity.PricePerItem * (1 - entity.DiscountPercent / 100);
        }
        else if (request.Amount.HasValue)
        {
            entity.Amount = request.Amount.Value;
            entity.DiscountPercent = 100 * (1 - entity.Amount / (entity.Quantity * entity.PricePerItem));
        }

        var result = await _provider.UpdateAsync(entity);

        await PublishMessageAsync(salaryId, EntityActionType.Updated);

        return _mapper.Map<SalaryLootDto>(result);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(long salaryId, long lootId)
    {
        var entity = await _provider.GetAsync(lootId, EntityTrackingType.NoTracking);
        if (entity is null)
        {
            throw new EntityNotFoundException();
        }

        await _provider.DeleteAsync(lootId);

        await PublishMessageAsync(salaryId, EntityActionType.Deleted);
    }

    private async Task PublishMessageAsync(long salaryId, EntityActionType actionType)
    {
        var message =
            new SalaryLootChangedMessage
            {
                SalaryId = salaryId, ActionType = actionType,
            };

        await _messageService.Publish(message);
    }
}
