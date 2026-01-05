using Freaks.Dal.Common.ValueObjects;
using Freaks.Messages.Bll.Interfaces;
using Freaks.Messages.SharedContracts.Messages.SalarySummary;
using Freaks.Messages.SharedContracts.ValueObjects;
using Freaks.Portal.Bll.Interfaces.SalarySummary;
using Freaks.Portal.Dal.Interfaces.SalarySummary;
using Freaks.Portal.SharedContracts.Dto.SalarySummary;
using Freaks.Portal.SharedContracts.Requests.SalarySummary.SalaryParameters;
using Freaks.WebApi.Common.Exceptions;
using MapsterMapper;

namespace Freaks.Portal.Bll.Implementation.SalarySummary;

/// <summary>
///     Сервис для управления параметрами зарплатных периодов.
///     Осуществляет бизнес-логику получения и обновления параметров расчета зарплаты,
///     а также маппинг сущностей в DTO и публикацию сообщений об изменениях.
/// </summary>
public class SalaryParametersService : ISalaryParametersService
{
    private readonly IMapper _mapper;
    private readonly ISalaryParametersProvider _provider;
    private readonly IMessageService _messageService;

    /// <summary>
    ///     Инициализирует новый экземпляр <see cref="SalaryParametersService"/>.
    /// </summary>
    /// <param name="mapper">Сервис Mapster для преобразования сущностей в DTO и обратно.</param>
    /// <param name="provider">Провайдер доступа к данным параметров зарплатных периодов.</param>
    /// <param name="messageService">Сервис для публикации сообщений в систему обмена сообщениями.</param>
    /// <exception cref="ArgumentNullException">Выбрасывается, если любой из аргументов равен null.</exception>
    public SalaryParametersService(
        IMapper mapper,
        ISalaryParametersProvider provider,
        IMessageService messageService)
    {
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _provider = provider ?? throw new ArgumentNullException(nameof(provider));
        _messageService = messageService ?? throw new ArgumentNullException(nameof(messageService));
    }

    /// <inheritdoc />
    public async Task<SalaryParametersDto> GetAsync(long salaryId)
    {
        var entity = await _provider.GetAsync(salaryId, EntityTrackingType.NoTracking);
        if (entity is null)
        {
            throw new EntityNotFoundException();
        }

        return _mapper.Map<SalaryParametersDto>(entity);
    }

    /// <inheritdoc />
    public async Task<SalaryParametersDto> UpdateAsync(long salaryId, UpdateSalaryParametersRequest request)
    {
        var entity = await _provider.GetAsync(salaryId, EntityTrackingType.NoTracking);
        if (entity is null)
        {
            throw new EntityNotFoundException();
        }

        // Обновляем все поля параметров
        entity.AllowedPaymentTypes = request.AllowedPaymentTypes;
        entity.UseCoefficients = request.UseCoefficients;
        entity.BossTypes = request.BossTypes;

        var result = await _provider.UpdateAsync(entity);

        await PublishMessageAsync(salaryId, EntityActionType.Updated);

        return _mapper.Map<SalaryParametersDto>(result);
    }

    private async Task PublishMessageAsync(long salaryId, EntityActionType actionType)
    {
        var message =
            new SalaryParametersChangedMessage
            {
                SalaryId = salaryId,
                ActionType = actionType,
            };

        await _messageService.Publish(message);
    }
}
