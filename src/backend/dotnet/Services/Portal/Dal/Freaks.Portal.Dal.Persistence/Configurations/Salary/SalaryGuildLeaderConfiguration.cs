using Freaks.Portal.Contracts.Entities.SalarySummary;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Freaks.Portal.Dal.Persistence.Configurations.Salary;

/// <inheritdoc />
public class SalaryGuildLeaderConfiguration : IEntityTypeConfiguration<SalaryGuildLeader>
{
    /// <inheritdoc />
    public void Configure(EntityTypeBuilder<SalaryGuildLeader> builder)
    {
        builder
            .HasOne(s => s.Salary)
            .WithMany()
            .HasForeignKey(s => s.SalaryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder
            .HasOne(s => s.SalaryLoot)
            .WithMany()
            .HasForeignKey(s => s.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}