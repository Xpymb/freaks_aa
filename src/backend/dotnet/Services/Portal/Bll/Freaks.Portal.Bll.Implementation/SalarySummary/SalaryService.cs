using Freaks.Dal.Common.ValueObjects;
using Freaks.Messages.Bll.Interfaces;
using Freaks.Messages.SharedContracts.Messages.SalarySummary;
using Freaks.Messages.SharedContracts.ValueObjects;
using Freaks.Portal.Bll.Interfaces.SalarySummary;
using Freaks.Portal.Contracts.Entities.SalarySummary;
using Freaks.Portal.Dal.Interfaces.SalarySummary;
using Freaks.Portal.SharedContracts.Dto.SalarySummary;
using Freaks.Portal.SharedContracts.Requests.SalarySummary.Salary;
using Freaks.Portal.SharedContracts.ValueObjects.RaidSummary;
using Freaks.Portal.SharedContracts.ValueObjects.SalarySummary;
using Freaks.SharedContracts.Common;
using Freaks.Users.Contracts.ValueObjects;
using Freaks.WebApi.Common.Exceptions;
using MapsterMapper;

namespace Freaks.Portal.Bll.Implementation.SalarySummary;

/// <summary>
///     Сервисный класс для управления зарплатными периодами.
///     Осуществляет взаимодействие между контроллерами и провайдером данных, включая маппинг и бизнес-логику.
/// </summary>
public class SalaryService : ISalaryService
{
    private readonly ISalaryProvider _provider;
    private readonly IMessageService _messageService;
    private readonly IMapper _mapper;
    private readonly IUserContext _userContext;

    /// <summary>
    ///     Инициализирует новый экземпляр сервиса <see cref="SalaryService" />.
    /// </summary>
    /// <param name="mapper">Сервис Mapster для преобразования между моделями.</param>
    /// <param name="userContext">Контекст текущего пользователя.</param>
    /// <param name="provider">Провайдер доступа к данным зарплатных периодов.</param>
    /// <param name="messageService">Сервис для публикации сообщений в систему обмена сообщениями.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если один из аргументов равен null.</exception>
    public SalaryService(
        IMapper mapper,
        IUserContext userContext,
        ISalaryProvider provider,
        IMessageService messageService)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _userContext = userContext ?? throw new ArgumentNullException(nameof(userContext));
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
    }

    /// <inheritdoc />
    public async Task<SalaryDto> GetAsync(long id)
    {
        var entity = await _provider.GetAsync(id, EntityTrackingType.NoTracking);
        if (entity is null)
        {
            throw new EntityNotFoundException();
        }

        return _mapper.Map<SalaryDto>(entity);
    }

    /// <inheritdoc />
    public async Task<PaginatedList<SalaryDto>> GetListAsync(GetSalaryListRequest request)
    {
        var result = await _provider.GetPaginatedListAsync(request);

        return _mapper.Map<PaginatedList<SalaryDto>>(result);
    }

    /// <inheritdoc />
    public async Task<SalaryDto> CreateAsync(CreateSalaryRequest request)
    {
        var entity =
            new Salary
            {
                Name = request.Name,
                StartDt = request.StartDt,
                EndDt = request.EndDt,
                FillStatus = SalaryFillStatus.Parameters,
                RegistrationStatus = SalaryRegistrationStatus.NotStarted,
                AllowedPaymentTypes = request.AllowedPaymentTypes,
                UseCoefficients = request.UseCoefficients,
                BossTypes = request.BossTypes,
                CreatedBy = _userContext.Id,
                CreatedDt = DateTime.UtcNow,
            };

        var result = await _provider.CreateAsync(entity);

        await PublishMessageAsync(result.Id, EntityActionType.Created);

        return _mapper.Map<SalaryDto>(result);
    }

    /// <inheritdoc />
    public async Task<SalaryDto> UpdateAsync(long id, UpdateSalaryRequest request)
    {
        var entity = await _provider.GetAsync(id, EntityTrackingType.NoTracking);
        if (entity is null)
        {
            throw new EntityNotFoundException();
        }

        entity.Name = request.Name;
        entity.StartDt = request.From;
        entity.EndDt = request.To;

        entity.UpdatedDt = DateTime.UtcNow;

        var result = await _provider.UpdateAsync(entity);

        await PublishMessageAsync(result.Id, EntityActionType.Updated);

        return _mapper.Map<SalaryDto>(result);
    }

    /// <inheritdoc />
    public async Task DeleteAsync(long id)
    {
        await _provider.DeleteAsync(id);

        await PublishMessageAsync(id, EntityActionType.Deleted);
    }

    /// <inheritdoc />
    public async Task<SalaryDto> FinishAsync(long id)
    {
        var entity = await _provider.GetAsync(id, EntityTrackingType.NoTracking);
        if (entity is null)
        {
            throw new EntityNotFoundException();
        }

        entity.FillStatus = SalaryFillStatus.FinalReports;

        entity.UpdatedDt = DateTime.UtcNow;

        var result = await _provider.UpdateAsync(entity);

        await PublishMessageAsync(result.Id, EntityActionType.Updated);

        return _mapper.Map<SalaryDto>(result);
    }

    /// <inheritdoc />
    public async Task<SalaryDto> OpenRegistrationAsync(long id)
    {
        var entity = await _provider.GetAsync(id, EntityTrackingType.NoTracking);
        if (entity is null)
        {
            throw new EntityNotFoundException();
        }

        entity.RegistrationStatus = SalaryRegistrationStatus.Opened;

        entity.UpdatedDt = DateTime.UtcNow;

        var result = await _provider.UpdateAsync(entity);

        await PublishMessageAsync(result.Id, EntityActionType.Updated);

        return _mapper.Map<SalaryDto>(result);
    }

    /// <inheritdoc />
    public async Task<SalaryDto> CloseRegistrationAsync(long id)
    {
        var entity = await _provider.GetAsync(id, EntityTrackingType.NoTracking);
        if (entity is null)
        {
            throw new EntityNotFoundException();
        }

        entity.RegistrationStatus = SalaryRegistrationStatus.Ended;

        entity.UpdatedDt = DateTime.UtcNow;

        var result = await _provider.UpdateAsync(entity);

        await PublishMessageAsync(result.Id, EntityActionType.Updated);

        return _mapper.Map<SalaryDto>(result);
    }

    private async Task PublishMessageAsync(long id, EntityActionType actionType)
    {
        var message =
            new SalaryChangedMessage
            {
                Id = id, ActionType = actionType,
            };

        await _messageService.Publish(message);
    }
}
