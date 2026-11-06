using Sergin.Administration.Settings.Domain.Languages;
using Sergin.SharedKernel.Domain.Repositories;

namespace Sergin.Administration.Settings.Domain.Translations;

/// <summary>(language, key) is unique; <c>ix_translation_language_id_key</c> is the guarantee.</summary>
public interface ITranslationRepository : IRepository<Translation, TranslationId>
{
    Task<Translation?> GetAsync(LanguageId languageId, TranslationKey key, CancellationToken cancellationToken = default);
}
