using Microsoft.EntityFrameworkCore;
using Sergin.Administration.Settings.Domain.Languages;
using Sergin.Administration.Settings.Domain.Translations;
using Sergin.Administration.Settings.Infrastructure.Data;
using Sergin.SharedKernel.Infrastructure.Data.EFCore.Repositories;

namespace Sergin.Administration.Settings.Infrastructure.Translations.Repositories;

internal class TranslationRepository(ISettingsDbContext dbContext)
    : EfRepository<Translation, TranslationId>(dbContext), ITranslationRepository
{
    public Task<Translation?> GetAsync(LanguageId languageId, TranslationKey key, CancellationToken cancellationToken = default) =>
        Set.SingleOrDefaultAsync(t => t.LanguageId == languageId && t.Key == key, cancellationToken);
}
