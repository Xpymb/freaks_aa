using Freaks.Dal.Common.ValueObjects;
using Freaks.Messages.Bll.Interfaces;
using Freaks.Messages.SharedContracts.Messages.SalarySummary;
using Freaks.Messages.SharedContracts.ValueObjects;
using Freaks.Portal.Bll.Interfaces.SalarySummary;
using Freaks.Portal.Contracts.Entities.SalarySummary;
using Freaks.Portal.Dal.Interfaces.SalarySummary;
using Freaks.Portal.SharedContracts.Dto.SalarySummary;
using Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryExpenses;
using Freaks.Portal.SharedContracts.ValueObjects.SalarySummary;
using Freaks.WebApi.Common.Exceptions;
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
    private readonly IMessageService _messageService;

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="SalaryExpensesService"/>.
    /// </summary>
    /// <param name="mapper">Сервис Mapster для преобразования сущностей в DTO и обратно.</param>
    /// <param name="provider">Провайдер доступа к данным расходов гильдии в зарплатных периодах.</param>
    /// <param name="messageService">Сервис для публикации сообщений в систему обмена сообщениями.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если любой из аргументов равен null.</exception>
    public SalaryExpensesService(
        IMapper mapper,
        ISalaryExpensesProvider provider,
        IMessageService messageService)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
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
        var entity =
            new SalaryExpenses
            {
                SalaryId = salaryId,
                ExpensesType = request.ExpensesType,
                Percentage = request.Percentage,
                Amount = request.Amount,
            };

        var result = await _provider.CreateAsync(entity);

        await PublishMessageAsync(salaryId, EntityActionType.Created);

        return _mapper.Map<SalaryExpensesDto>(result!);
    }

    /// <inheritdoc />
    public async Task<SalaryExpensesDto> UpdateAsync(long salaryId, SalaryExpensesType expensesType, UpdateSalaryExpensesRequest request)
    {
        var key = new SalaryExpensesKey(salaryId, expensesType);
        var entity = await _provider.GetAsync(key, EntityTrackingType.NoTracking);
        if (entity is null)
        {
            throw new EntityNotFoundException();
        }

        entity.Percentage = request.Percentage;
        entity.Amount = request.Amount;

        var result = await _provider.UpdateAsync(entity);

        await PublishMessageAsync(salaryId, EntityActionType.Updated);

        return _mapper.Map<SalaryExpensesDto>(result);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(long salaryId, SalaryExpensesType expensesType)
    {
        var key = new SalaryExpensesKey(salaryId, expensesType);
        var entity = await _provider.GetAsync(key, EntityTrackingType.NoTracking);
        if (entity is null)
        {
            throw new EntityNotFoundException();
        }

        await _provider.DeleteAsync(key);

        await PublishMessageAsync(salaryId, EntityActionType.Deleted);
    }

    private async Task PublishMessageAsync(long salaryId, EntityActionType actionType)
    {
        var message =
            new SalaryExpensesChangedMessage
            {
                SalaryId = salaryId, ActionType = actionType,
            };

        await _messageService.Publish(message);
    }
}
