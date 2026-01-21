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
using Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryExpenses;
using Freaks.Portal.SharedContracts.ValueObjects.SalarySummary;
using Freaks.WebApi.Common.Exceptions;
using Freaks.WebApi.Common.Exceptions.Salary;
using MapsterMapper;

namespace Freaks.Portal.Bll.Implementation.SalarySummary;

/// <summary>
///     Сервис для управления расходами гильдии в зарплатных периодах.
///     Осуществляет бизнес-логику добавления, обновления, удаления и получения записей о расходах,
///     а также маппинг сущностей в DTO. Работает с составным ключом (SalaryId + ExpensesType).
/// </summary>
public class SalaryExpensesService : ISalaryExpensesService
{
    private readonly IMapper _mapper;
    private readonly ISalaryExpensesProvider _provider;
    private readonly ISalaryLootProvider _lootProvider;
    private readonly ISalaryStepService _stepService;
    private readonly IMessageService _messageService;
    private readonly IUnitOfWork<PortalDbContext> _unitOfWork;

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="SalaryExpensesService"/>.
    /// </summary>
    /// <param name="mapper">Сервис Mapster для преобразования сущностей в DTO и обратно.</param>
    /// <param name="provider">Провайдер доступа к данным расходов гильдии в зарплатных периодах.</param>
    /// <param name="lootProvider">Провайдер доступа к данным проданного лута зарплатных периодов.</param>
    /// <param name="stepService">Сервис степпера зарплатных периодов.</param>
    /// <param name="messageService">Сервис для публикации сообщений в систему обмена сообщениями.</param>
    /// <param name="unitOfWork">Unit of Work</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если любой из аргументов равен null.</exception>
    public SalaryExpensesService(
        IMapper mapper,
        ISalaryExpensesProvider provider,
        ISalaryLootProvider lootProvider,
        ISalaryStepService stepService,
        IMessageService messageService,
        IUnitOfWork<PortalDbContext> unitOfWork)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _lootProvider = lootProvider ?? throw new ArgumentNullException(nameof(lootProvider));
        _stepService = stepService ?? throw new ArgumentNullException(nameof(stepService));
        _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    /// <inheritdoc />
    public async Task<IList<SalaryExpensesDto>> GetListAsync(long salaryId)
    {
        var result = await _provider.GetBySalaryIdAsync(salaryId);

        return _mapper.Map<IList<SalaryExpensesDto>>(result);
    }

    /// <inheritdoc />
    public async Task<SalaryExpensesDto> CreateAsync(long salaryId, CreateSalaryExpensesRequest request)
    {
        return await _unitOfWork.ExecuteInsideTransactionAsync(async _ =>
        {
            await _stepService.HandleStepActionAsync(salaryId, SalaryFillStepType.Expenses);

            if (request.ExpensesType is SalaryExpensesType.TargetMember && request.UserId is null)
            {
                throw new SalaryExpensesUserShouldBeAssignedException();
            }

            var salaryLoot = await _lootProvider.GetBySalaryIdAsync(salaryId);

            var percentage = 0m;
            var amount = 0m;
            if (request.Amount is not null && salaryLoot.Count > 0)
            {
                amount = request.Amount.Value;
                percentage = amount * 100 / salaryLoot.Sum(x => x.Amount);
            }
            else if (request.Percentage is not null && salaryLoot.Count > 0)
            {
                percentage = request.Percentage.Value;
                amount = percentage / 100 * salaryLoot.Sum(x => x.Amount);
            }

            var entity =
                new SalaryExpenses
                {
                    SalaryId = salaryId,
                    ExpensesType = request.ExpensesType,
                    UserId = request.UserId,
                    Percentage = percentage,
                    Amount = amount,
                };

            var result = await _provider.CreateAsync(entity);

            await PublishMessageAsync(salaryId, EntityActionType.Created);

            return _mapper.Map<SalaryExpensesDto>(result);
        });
    }

    /// <inheritdoc />
    public async Task<SalaryExpensesDto> UpdateAsync(long id, long salaryId, UpdateSalaryExpensesRequest request)
    {
        return await _unitOfWork.ExecuteInsideTransactionAsync(async _ =>
        {
            await _stepService.HandleStepActionAsync(salaryId, SalaryFillStepType.Expenses);

            var entity = await _provider.GetAsync(id, EntityTrackingType.NoTracking);
            if (entity is null)
            {
                throw new EntityNotFoundException(nameof(SalaryExpenses));
            }

            var salaryLoot = await _lootProvider.GetBySalaryIdAsync(salaryId);

            if (entity.Amount - request.Amount != 0 && salaryLoot.Count > 0)
            {
                entity.Percentage = request.Amount * 100 / salaryLoot.Sum(x => x.Amount);
            }
            else if (entity.Percentage - request.Percentage != 0 && salaryLoot.Count > 0)
            {
                entity.Amount = request.Percentage / 100 * salaryLoot.Sum(x => x.Amount);
            }

            var result = await _provider.UpdateAsync(entity);

            await PublishMessageAsync(salaryId, EntityActionType.Updated);

            return _mapper.Map<SalaryExpensesDto>(result);
        });
    }

    /// <inheritdoc />
    public async Task DeleteAsync(long id, long salaryId)
    {
        await _unitOfWork.ExecuteInsideTransactionAsync(async _ =>
        {
            await _stepService.HandleStepActionAsync(salaryId, SalaryFillStepType.Expenses);

            var entity = await _provider.GetAsync(id, EntityTrackingType.NoTracking);
            if (entity is null)
            {
                return;
            }

            await _provider.DeleteAsync(id);

            await PublishMessageAsync(id, EntityActionType.Deleted);
        });
    }

    private async Task PublishMessageAsync(long id, EntityActionType actionType)
    {
        var message =
            new SalaryExpensesChangedMessage
            {
                Id = id, ActionType = actionType,
            };

        await _messageService.Publish(message);
    }
}
