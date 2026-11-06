using Microsoft.EntityFrameworkCore;
using Sergin.Administration.Settings.Application;
using Sergin.Administration.Settings.Application.Configurations;
using Sergin.SharedKernel.Application.Aggregates;
using Sergin.SharedKernel.Infrastructure.Data.EFCore;

namespace Sergin.Administration.Settings.Infrastructure.Data;

public interface ISettingsDbContext : IDbContext;

internal sealed class SettingsDbContext(DbContextOptions<SettingsDbContext> options)
    : SerginDbContext(options), ISettingsDbContext, ISettingsUnitOfWork
{
    public const string Schema = "st";

    protected override AggregateFeatureRegistry AggregateFeatures =>
        AggregateFeatureRegistry.FromAssemblies([SettingsApplicationConfigurationsAssemblyReference.Assembly]);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);

        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
