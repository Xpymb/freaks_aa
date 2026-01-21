using Freaks.Portal.Contracts.Entities.SalarySummary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freaks.Portal.Dal.Persistence.Configurations.Salary;

/// <inheritdoc />
public class SalaryExpensesConfiguration : IEntityTypeConfiguration<SalaryExpenses>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<SalaryExpenses> builder)
    {
        builder
            .HasOne(s => s.Salary)
            .WithMany()
            .HasForeignKey(s => s.SalaryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(s => s.User)
            .WithMany()
            .HasForeignKey(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}