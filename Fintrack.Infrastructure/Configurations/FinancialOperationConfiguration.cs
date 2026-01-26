using Fintrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fintrack.Infrastructure.Configurations;

internal sealed class FinancialOperationConfiguration : IEntityTypeConfiguration<FinancialOperation>
{
    public void Configure(EntityTypeBuilder<FinancialOperation> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Amount)
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(e => e.Date)
            .IsRequired();

        builder.Property(e => e.UserComment)
            .HasMaxLength(500);

        builder.HasOne(e => e.TransactionType)
            .WithMany(e => e.FinancialOperations)
            .HasForeignKey(e => e.TransactionTypeId);

        builder.HasOne(e => e.User)
            .WithMany(e => e.FinancialOperations)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
