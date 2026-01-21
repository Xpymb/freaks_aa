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
using Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryGuildLeader;
using Freaks.Portal.SharedContracts.ValueObjects.SalarySummary;
using Freaks.WebApi.Common.Exceptions;
using MapsterMapper;

namespace Freaks.Portal.Bll.Implementation.SalarySummary;

/// <summary>
///     Сервис для управления долями руководства гильдии в зарплатных периодах.
///     Осуществляет бизнес-логику добавления, обновления, удаления и получения записей о долях руководства,
///     а также маппинг сущностей в DTO. Включает простую финансовую логику: Amount = Quantity * PricePerLoot.
/// </summary>
public class SalaryGuildLeaderService : ISalaryGuildLeaderService
{
    private readonly IMapper _mapper;
    private readonly ISalaryGuildLeaderProvider _provider;
    private readonly ISalaryStepService _stepService;
    private readonly IMessageService _messageService;
    private readonly IUnitOfWork<PortalDbContext> _unitOfWork;

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="SalaryGuildLeaderService"/>.
    /// </summary>
    /// <param name="mapper">Сервис Mapster для преобразования сущностей в DTO и обратно.</param>
    /// <param name="provider">Провайдер доступа к данным долей руководства гильдии в зарплатных периодах.</param>
    /// <param name="stepService">Сервис степпера зарплатных периодов.</param>
    /// <param name="messageService">Сервис для публикации сообщений в систему обмена сообщениями.</param>
    /// <param name="unitOfWork">Unit of Work</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если любой из аргументов равен null.</exception>
    public SalaryGuildLeaderService(
        IMapper mapper,
        ISalaryGuildLeaderProvider provider,
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
    public async Task<IList<SalaryGuildLeaderDto>> GetListAsync(long salaryId)
    {
        var result = await _provider.GetBySalaryIdAsync(salaryId);

        return _mapper.Map<IList<SalaryGuildLeaderDto>>(result);
    }

    /// <inheritdoc />
    public async Task<SalaryGuildLeaderDto> CreateAsync(long salaryId, CreateSalaryGuildLeaderRequest request)
    {
        return await _unitOfWork.ExecuteInsideTransactionAsync(async _ =>
        {
            await _stepService.HandleStepActionAsync(salaryId, SalaryFillStepType.GuildLeaderSalary);

            var amount = request.Quantity * request.PricePerItem;

            var entity =
                new SalaryGuildLeader
                {
                    SalaryId = salaryId,
                    LootId = request.LootId,
                    Quantity = request.Quantity,
                    PricePerItem = request.PricePerItem,
                    Amount = amount,
                };

            var result = await _provider.CreateAsync(entity);

            await PublishMessageAsync(salaryId, EntityActionType.Created);

            return _mapper.Map<SalaryGuildLeaderDto>(result);
        });
    }

    /// <inheritdoc />
    public async Task<SalaryGuildLeaderDto> UpdateAsync(long salaryId, long guildLeaderId, UpdateSalaryGuildLeaderRequest request)
    {
        return await _unitOfWork.ExecuteInsideTransactionAsync(async _ =>
        {
            await _stepService.HandleStepActionAsync(salaryId, SalaryFillStepType.GuildLeaderSalary);

            var entity = await _provider.GetAsync(guildLeaderId, EntityTrackingType.NoTracking);
            if (entity is null)
            {
                throw new EntityNotFoundException(nameof(SalaryGuildLeader));
            }

            entity.LootId = request.LootId;
            entity.Quantity = request.Quantity;
            entity.PricePerItem = request.PricePerItem;

            entity.Amount = entity.Quantity * entity.PricePerItem;

            var result = await _provider.UpdateAsync(entity);

            await PublishMessageAsync(salaryId, EntityActionType.Updated);

            return _mapper.Map<SalaryGuildLeaderDto>(result);
        });
    }

    /// <inheritdoc />
    public async Task DeleteAsync(long salaryId, long guildLeaderId)
    {
        await _unitOfWork.ExecuteInsideTransactionAsync(async _ =>
        {
            await _stepService.HandleStepActionAsync(salaryId, SalaryFillStepType.GuildLeaderSalary);

            var entity = await _provider.GetAsync(guildLeaderId, EntityTrackingType.NoTracking);
            if (entity is null)
            {
                return;
            }

            await _provider.DeleteAsync(guildLeaderId);

            await PublishMessageAsync(salaryId, EntityActionType.Deleted);
        });
    }

    private async Task PublishMessageAsync(long salaryId, EntityActionType actionType)
    {
        var message =
            new SalaryGuildLeaderChangedMessage
            {
                SalaryId = salaryId, ActionType = actionType,
            };

        await _messageService.Publish(message);
    }
}
