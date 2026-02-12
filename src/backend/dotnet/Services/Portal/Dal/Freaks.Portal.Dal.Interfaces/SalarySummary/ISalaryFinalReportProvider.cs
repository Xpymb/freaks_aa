using Freaks.Dal.Common.Interfaces;
using Freaks.Portal.Contracts.Entities.SalarySummary;
using Freaks.Portal.Dal.Persistence;

namespace Freaks.Portal.Dal.Interfaces.SalarySummary;

/// <summary>
///     Интерфейс провайдера для работы с итоговыми отчётами зарплатных периодов.
/// </summary>
public interface ISalaryFinalReportProvider : IBaseProvider<SalaryFinalReport, long, IPortalDbContext>
{
}