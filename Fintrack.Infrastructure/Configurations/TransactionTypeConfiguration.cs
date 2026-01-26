using Fintrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fintrack.Infrastructure.Configurations;

internal sealed class TransactionTypeConfiguration : IEntityTypeConfiguration<TransactionType>
{
    public void Configure(EntityTypeBuilder<TransactionType> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.IsExpense)
            .IsRequired();

        builder.Property(e => e.UserId)
            .IsRequired();

        builder.HasOne(e => e.User)
            .WithMany(e => e.TransactionTypes)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(e => e.Mccs)
            .WithMany(e => e.TransactionTypes);

        builder.HasMany(e => e.FinancialOperations)
            .WithOne(e => e.TransactionType)
            .HasForeignKey(e => e.TransactionTypeId);
    }
}
