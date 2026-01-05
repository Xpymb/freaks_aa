using Freaks.Portal.Contracts.Entities.SalarySummary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freaks.Portal.Dal.Persistence.Configurations.SalarySummary;

/// <inheritdoc />
public class SalaryConfiguration : IEntityTypeConfiguration<Salary>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<Salary> builder)
    {
    }
}