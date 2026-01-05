using Freaks.Portal.Contracts.Entities.SalarySummary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freaks.Portal.Dal.Persistence.Configurations.SalarySummary;

/// <inheritdoc />
public class SalaryExpensesConfiguration : IEntityTypeConfiguration<SalaryExpenses>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<SalaryExpenses> builder)
    {
        builder.HasKey(s => new { s.SalaryId, s.ExpensesType });

        builder
            .HasOne(s => s.Salary)
            .WithMany()
            .HasForeignKey(s => s.SalaryId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}