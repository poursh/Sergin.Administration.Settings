using Microsoft.EntityFrameworkCore;
using Sergin.Platform.Application;
using Sergin.Platform.Application.Configurations;
using Sergin.SharedKernel.Application.Aggregates;
using Sergin.SharedKernel.Infrastructure.Data.EFCore;

namespace Sergin.Platform.Infrastructure.Data;

public interface IPlatformDbContext : IDbContext;

internal sealed class PlatformDbContext(DbContextOptions<PlatformDbContext> options)
    : SerginDbContext(options), IPlatformDbContext, IPlatformUnitOfWork
{
    public const string Schema = "pf";

    protected override AggregateFeatureRegistry AggregateFeatures =>
        AggregateFeatureRegistry.FromAssemblies([PlatformApplicationConfigurationsAssemblyReference.Assembly]);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema(Schema);

        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);

        base.OnModelCreating(modelBuilder);
    }
}
