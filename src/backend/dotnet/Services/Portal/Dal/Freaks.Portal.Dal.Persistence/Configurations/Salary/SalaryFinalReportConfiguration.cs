using Freaks.Portal.Contracts.Entities.SalarySummary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freaks.Portal.Dal.Persistence.Configurations.Salary;

/// <summary>
///     Конфигурация EF Core для сущности <see cref="SalaryFinalReport" />.
/// </summary>
public class SalaryFinalReportConfiguration : IEntityTypeConfiguration<SalaryFinalReport>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<SalaryFinalReport> builder)
    {
        builder
            .HasOne(s => s.Salary)
            .WithOne()
            .HasForeignKey<SalaryFinalReport>(s => s.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}