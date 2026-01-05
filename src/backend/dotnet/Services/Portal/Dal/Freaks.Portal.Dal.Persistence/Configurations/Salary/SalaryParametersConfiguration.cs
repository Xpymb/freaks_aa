using Freaks.Portal.Contracts.Entities.SalarySummary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freaks.Portal.Dal.Persistence.Configurations.SalarySummary;

/// <inheritdoc />
public class SalaryParametersConfiguration : IEntityTypeConfiguration<SalaryParameters>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<SalaryParameters> builder)
    {
        builder
            .HasOne(s => s.Salary)
            .WithMany()
            .HasForeignKey(s => s.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}