using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sergin.Platform.Infrastructure.Data;

internal sealed class PlatformDbContextDesignTimeFactory() : IDesignTimeDbContextFactory<PlatformDbContext>
{
    public PlatformDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Development.json", false, false)
            .Build();

        string connectionString = configuration.GetSection("Sergin").GetConnectionString("Database");

        var optionBuilder = new DbContextOptionsBuilder<PlatformDbContext>();

        optionBuilder
            .UseNpgsql(connectionString,
                    sqlOptions => sqlOptions
                    .MigrationsHistoryTable(HistoryRepository.DefaultTableName, PlatformDbContext.Schema))
            .UseSnakeCaseNamingConvention();

        return new PlatformDbContext(optionBuilder.Options);
    }
}
