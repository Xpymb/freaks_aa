using Freaks.Dal.Common.Interfaces;
using Freaks.Portal.Contracts.Entities.SalarySummary;
using Freaks.Portal.Dal.Persistence;

namespace Freaks.Portal.Dal.Interfaces.SalarySummary;

/// <summary>
///     Провайдер для работы с параметрами зарплатного периода.
/// </summary>
public interface ISalaryParametersProvider : IBaseProvider<SalaryParameters, long, IPortalDbContext>
{
}