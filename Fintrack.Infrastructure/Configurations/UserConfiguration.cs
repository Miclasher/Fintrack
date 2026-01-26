using Fintrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fintrack.Infrastructure.Configurations;

internal sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Username)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.Email)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(e => e.PasswordHash)
            .IsRequired();

        builder.Property(e => e.Salt)
            .IsRequired();

        builder.Property(e => e.DisplayName)
            .HasMaxLength(100);

        builder.HasMany(e => e.FinancialOperations)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId);

        builder.HasMany(e => e.TransactionTypes)
            .WithOne(e => e.User)
            .HasForeignKey(e => e.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
