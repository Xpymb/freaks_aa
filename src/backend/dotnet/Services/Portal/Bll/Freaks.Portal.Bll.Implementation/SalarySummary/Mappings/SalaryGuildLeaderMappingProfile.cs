using Freaks.Portal.Bll.Implementation.SalarySummary.Helpers;
using Freaks.Portal.Contracts.Entities.SalarySummary;
using Freaks.Portal.SharedContracts.Dto.SalarySummary;
using Mapster;

namespace Freaks.Portal.Bll.Implementation.SalarySummary.Mappings;

public class SalaryGuildLeaderMappingProfile : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<SalaryGuildLeader, SalaryGuildLeaderDto>()
            .Map(dest => dest.Amount, src => SalaryGuildLeaderHelper.CalculateAmount(src.SalaryLoot, src.Quantity));
    }
}