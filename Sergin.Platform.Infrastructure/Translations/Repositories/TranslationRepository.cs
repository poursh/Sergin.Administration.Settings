using Microsoft.EntityFrameworkCore;
using Sergin.Platform.Domain.Languages;
using Sergin.Platform.Domain.Translations;
using Sergin.Platform.Infrastructure.Data;
using Sergin.SharedKernel.Infrastructure.Data.EFCore.Repositories;

namespace Sergin.Platform.Infrastructure.Translations.Repositories;

internal class TranslationRepository(IPlatformDbContext dbContext)
    : EfRepository<Translation, TranslationId>(dbContext), ITranslationRepository
{
    public Task<Translation?> GetAsync(LanguageId languageId, TranslationKey key, CancellationToken cancellationToken = default) =>
        Set.SingleOrDefaultAsync(t => t.LanguageId == languageId && t.Key == key, cancellationToken);
}
