using Fintrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Fintrack.Infrastructure.Configurations;

internal sealed class JwtRefreshTokenConfiguration : IEntityTypeConfiguration<JwtRefreshToken>
{
    public void Configure(EntityTypeBuilder<JwtRefreshToken> builder)
    {
        builder.HasKey(e => e.Id);

        builder.HasIndex(e => e.Token).IsUnique();

        builder.Property(e => e.Token)
            .IsRequired();

        builder.Property(e => e.ExpiryDate)
            .IsRequired();

        builder.Property(e => e.CreatedAt)
            .IsRequired();

        builder.HasOne(e => e.User).WithOne()
            .HasForeignKey<JwtRefreshToken>(e => e.UserId);
    }
}
