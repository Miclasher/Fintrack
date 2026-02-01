using Fintrack.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Fintrack.Infrastructure;

public sealed class FintrackContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<JwtRefreshToken> RefreshTokens { get; set; }
    public DbSet<FinancialOperation> FinancialOperations { get; set; }
    public DbSet<TransactionType> TransactionTypes { get; set; }
    public DbSet<TransactionTypeTemplate> TransactionTypeTemplates { get; set; }
    public DbSet<Mcc> Mccs { get; set; }
    public FintrackContext(DbContextOptions<FintrackContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(FintrackContext).Assembly,
            type => type.GetInterfaces().Any(i =>
                i.IsGenericType &&
                i.GetGenericTypeDefinition() == typeof(IEntityTypeConfiguration<>))
        );
    }
}
