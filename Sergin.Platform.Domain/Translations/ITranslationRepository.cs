using Sergin.Platform.Domain.Languages;
using Sergin.SharedKernel.Domain.Repositories;

namespace Sergin.Platform.Domain.Translations;

/// <summary>(language, key) is unique; <c>ix_translation_language_id_key</c> is the guarantee.</summary>
public interface ITranslationRepository : IRepository<Translation, TranslationId>
{
    Task<Translation?> GetAsync(LanguageId languageId, TranslationKey key, CancellationToken cancellationToken = default);
}
