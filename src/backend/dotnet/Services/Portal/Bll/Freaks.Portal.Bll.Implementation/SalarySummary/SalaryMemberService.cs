using Freaks.Dal.Common.ValueObjects;
using Freaks.Messages.Bll.Interfaces;
using Freaks.Messages.SharedContracts.Messages.SalarySummary;
using Freaks.Messages.SharedContracts.ValueObjects;
using Freaks.Portal.Bll.Interfaces.SalarySummary;
using Freaks.Portal.Contracts.Entities.SalarySummary;
using Freaks.Portal.Dal.Interfaces.SalarySummary;
using Freaks.Portal.SharedContracts.Dto.SalarySummary;
using Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryMember;
using Freaks.Users.Contracts.ValueObjects;
using Freaks.WebApi.Common.Exceptions;
using MapsterMapper;

namespace Freaks.Portal.Bll.Implementation.SalarySummary;

/// <summary>
///     Сервис для управления участниками зарплатных периодов.
///     Осуществляет бизнес-логику добавления, обновления, удаления и получения записей об участниках,
///     а также маппинг сущностей в DTO. Работает с составным ключом (SalaryId + UserId).
/// </summary>
public class SalaryMemberService : ISalaryMemberService
{
    private readonly IMapper _mapper;
    private readonly ISalaryMemberProvider _provider;
    private readonly IMessageService _messageService;
    private readonly IUserContext _userContext;

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="SalaryMemberService"/>.
    /// </summary>
    /// <param name="mapper">Сервис Mapster для преобразования сущностей в DTO и обратно.</param>
    /// <param name="provider">Провайдер доступа к данным участников зарплатных периодов.</param>
    /// <param name="messageService">Сервис для публикации сообщений в систему обмена сообщениями.</param>
    /// <param name="userContext">Контекст текущего пользователя.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если любой из аргументов равен null.</exception>
    public SalaryMemberService(
        IMapper mapper,
        ISalaryMemberProvider provider,
        IMessageService messageService,
        IUserContext userContext)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
        _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
    }

    /// <inheritdoc />
    public async Task<IList<SalaryMemberDto>> GetListAsync(long salaryId)
    {
        var result = await _provider.GetBySalaryIdAsync(salaryId);

        return _mapper.Map<IList<SalaryMemberDto>>(result);
    }

    /// <inheritdoc />
    public async Task<SalaryMemberDto> CreateAsync(long salaryId, CreateSalaryMemberRequest request)
    {
        var currentUserId = _userContext.Id;

        var entity =
            new SalaryMember
            {
                SalaryId = salaryId,
                UserId = currentUserId,
                PaymentType = request.PaymentType,
                ActivityPercentage = request.ActivityPercentage,
                Coefficient = request.Coefficient,
                ActivityGold = null,
                ResponsibilityGold = null,
                AmountGold = null,
                AmountWorldBossInfusion = null,
            };

        var result = await _provider.CreateAsync(entity);

        await PublishMessageAsync(salaryId, EntityActionType.Created);

        return _mapper.Map<SalaryMemberDto>(result!);
    }

    /// <inheritdoc />
    public async Task<SalaryMemberDto> CreateAdminAsync(long salaryId, CreateSalaryMemberAdminRequest request)
    {
        var entity =
            new SalaryMember
            {
                SalaryId = salaryId,
                UserId = request.UserId,
                PaymentType = request.PaymentType,
                ActivityPercentage = request.ActivityPercentage,
                Coefficient = request.Coefficient,
                ActivityGold = null,
                ResponsibilityGold = null,
                AmountGold = null,
                AmountWorldBossInfusion = null,
            };

        var result = await _provider.CreateAsync(entity);

        await PublishMessageAsync(salaryId, EntityActionType.Created);

        return _mapper.Map<SalaryMemberDto>(result!);
    }

    /// <inheritdoc />
    public async Task<SalaryMemberDto> UpdateAsync(long salaryId, Guid userId, UpdateSalaryMemberRequest request)
    {
        var key = new SalaryMemberKey(salaryId, userId);
        var entity = await _provider.GetAsync(key, EntityTrackingType.NoTracking);
        if (entity is null)
        {
            throw new EntityNotFoundException();
        }

        entity.PaymentType = request.PaymentType;
        entity.ActivityPercentage = request.ActivityPercentage;
        entity.Coefficient = request.Coefficient;

        var result = await _provider.UpdateAsync(entity);

        await PublishMessageAsync(salaryId, EntityActionType.Updated);

        return _mapper.Map<SalaryMemberDto>(result);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(long salaryId, Guid userId)
    {
        var key = new SalaryMemberKey(salaryId, userId);
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
            new SalaryMemberChangedMessage
            {
                SalaryId = salaryId, ActionType = actionType,
            };

        await _messageService.Publish(message);
    }
}
