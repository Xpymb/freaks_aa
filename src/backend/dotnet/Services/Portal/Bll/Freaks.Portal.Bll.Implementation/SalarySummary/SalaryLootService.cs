using Freaks.Dal.Common.Interfaces;
using Freaks.Dal.Common.ValueObjects;
using Freaks.Messages.Bll.Interfaces;
using Freaks.Messages.SharedContracts.Messages.SalarySummary;
using Freaks.Messages.SharedContracts.ValueObjects;
using Freaks.Portal.Bll.Interfaces.SalarySummary;
using Freaks.Portal.Contracts.Entities.SalarySummary;
using Freaks.Portal.Dal.Interfaces.SalarySummary;
using Freaks.Portal.Dal.Persistence;
using Freaks.Portal.SharedContracts.Dto.SalarySummary;
using Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryLoot;
using Freaks.Portal.SharedContracts.ValueObjects.SalarySummary;
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
    private readonly ISalaryStepService _stepService;
    private readonly IMessageService _messageService;
    private readonly IUnitOfWork<PortalDbContext> _unitOfWork;

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="SalaryLootService"/>.
    /// </summary>
    /// <param name="mapper">Сервис Mapster для преобразования сущностей в DTO и обратно.</param>
    /// <param name="provider">Провайдер доступа к данным проданного лута зарплатных периодов.</param>
    /// <param name="stepService">Сервис степпера зарплатных периодов.</param>
    /// <param name="messageService">Сервис для публикации сообщений в систему обмена сообщениями.</param>
    /// <param name="unitOfWork">Unit of Work</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если любой из аргументов равен null.</exception>
    public SalaryLootService(
        IMapper mapper,
        ISalaryLootProvider provider,
        ISalaryStepService stepService,
        IMessageService messageService,
        IUnitOfWork<PortalDbContext> unitOfWork)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _stepService = stepService ?? throw new ArgumentNullException(nameof(stepService));
        _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
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
        return await _unitOfWork.ExecuteInsideTransactionAsync(async _ =>
        {
            await _stepService.HandleStepActionAsync(salaryId, SalaryFillStepType.SoldByPeriod);

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

            return _mapper.Map<SalaryLootDto>(result);
        });
    }

    /// <inheritdoc />
    public async Task<SalaryLootDto> UpdateAsync(long salaryId, long lootId, UpdateSalaryLootRequest request)
    {
        return await _unitOfWork.ExecuteInsideTransactionAsync(async _ =>
        {
            await _stepService.HandleStepActionAsync(salaryId, SalaryFillStepType.SoldByPeriod);

            var entity = await _provider.GetAsync(lootId, EntityTrackingType.NoTracking);
            if (entity is null)
            {
                throw new EntityNotFoundException(nameof(SalaryLoot));
            }

            entity.LootId = request.LootId;
            entity.Quantity = request.Quantity;
            entity.PricePerItem = request.PricePerItem;

            if (request.DiscountPercent > 0)
            {
                entity.DiscountPercent = request.DiscountPercent;
                entity.Amount = entity.Quantity * entity.PricePerItem * (1 - entity.DiscountPercent / 100);
            }
            else
            {
                entity.DiscountPercent = 0;
                entity.Amount = entity.Quantity * entity.PricePerItem;
            }

            var result = await _provider.UpdateAsync(entity);

            await PublishMessageAsync(salaryId, EntityActionType.Updated);

            return _mapper.Map<SalaryLootDto>(result);
        });
    }

    /// <inheritdoc />
    public async Task DeleteAsync(long salaryId, long lootId)
    {
        await _unitOfWork.ExecuteInsideTransactionAsync(async _ =>
        {
            await _stepService.HandleStepActionAsync(salaryId, SalaryFillStepType.SoldByPeriod);

            var entity = await _provider.GetAsync(lootId, EntityTrackingType.NoTracking);
            if (entity is null)
            {
                return;
            }

            await _provider.DeleteAsync(lootId);

            await PublishMessageAsync(salaryId, EntityActionType.Deleted);
        });
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
