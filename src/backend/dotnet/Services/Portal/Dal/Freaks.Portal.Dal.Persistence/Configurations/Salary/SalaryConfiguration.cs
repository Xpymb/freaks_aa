using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freaks.Portal.Dal.Persistence.Configurations.Salary;

/// <inheritdoc />
public class SalaryConfiguration : IEntityTypeConfiguration<Contracts.Entities.SalarySummary.Salary>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Contracts.Entities.SalarySummary.Salary> builder)
    {
    }
}