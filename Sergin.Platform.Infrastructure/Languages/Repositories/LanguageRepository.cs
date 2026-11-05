using Sergin.Platform.Domain.Languages;
using Sergin.Platform.Infrastructure.Data;
using Sergin.SharedKernel.Infrastructure.Data.EFCore.Repositories;

namespace Sergin.Platform.Infrastructure.Languages.Repositories;

internal class LanguageRepository(IPlatformDbContext dbContext)
    : EfRepository<Language, LanguageId>(dbContext), ILanguageRepository
{
    // Compared after normalisation, the way Language.Create stores it: "FA-ir" is taken once "fa-IR" is. A culture .NET
    // does not know is compared as given; the validator refuses it before this runs.
    public Task<bool> IsTakenAsync(CultureName key, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(key);

        CultureName normalised = CultureName.Resolve(key.Value) is { } culture ? new CultureName(culture.Name) : key;

        return AnyAsync(l => l.Culture == normalised, cancellationToken);
    }
}
