using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Fintrack.Infrastructure;

public class FintrackContextFactory : IDesignTimeDbContextFactory<FintrackContext>
{
    public FintrackContext CreateDbContext(string[] args)
    {
        var config = new ConfigurationBuilder().AddUserSecrets<FintrackContextFactory>().Build();

        var options = new DbContextOptionsBuilder<FintrackContext>()
            .UseSqlServer(config["DbConnectionString"])
            .Options;

        return new FintrackContext(options);
    }
}
