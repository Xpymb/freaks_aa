using Freaks.Portal.Bll.Implementation.SalarySummary.Helpers;
using Freaks.Portal.Contracts.Entities.SalarySummary;
using Freaks.Portal.SharedContracts.Dto.SalarySummary;
using Mapster;

namespace Freaks.Portal.Bll.Implementation.SalarySummary.Mappings;

/// <summary>
///     Профиль маппинга для преобразования сущности <see cref="SalaryGuildLeader"/> в DTO <see cref="SalaryGuildLeaderDto"/>.
/// </summary>
public class SalaryGuildLeaderMappingProfile : IRegister
{
    /// <summary>
    ///     Регистрирует правила маппинга для доли руководства гильдии в конфигурации Mapster.
    /// </summary>
    /// <param name="config">Конфигурация Mapster для регистрации правил маппинга.</param>
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<SalaryGuildLeader, SalaryGuildLeaderDto>()
            .Map(dest => dest.Amount, src => SalaryGuildLeaderHelper.CalculateAmount(src.SalaryLoot, src.Quantity));
    }
}