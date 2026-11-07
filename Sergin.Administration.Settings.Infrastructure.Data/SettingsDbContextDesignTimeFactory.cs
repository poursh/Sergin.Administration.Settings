using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Sergin.Administration.Settings.Infrastructure.Data;

internal sealed class SettingsDbContextDesignTimeFactory() : IDesignTimeDbContextFactory<SettingsDbContext>
{
    public SettingsDbContext CreateDbContext(string[] args)
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.Development.json", false, false)
            .Build();

        string connectionString = configuration.GetSection("Sergin").GetConnectionString("Database");

        var optionBuilder = new DbContextOptionsBuilder<SettingsDbContext>();

        optionBuilder
            .UseNpgsql(connectionString,
                    sqlOptions => sqlOptions
                    .MigrationsHistoryTable(HistoryRepository.DefaultTableName, SettingsDbContext.Schema))
            .UseSnakeCaseNamingConvention();

        return new SettingsDbContext(optionBuilder.Options);
    }
}
