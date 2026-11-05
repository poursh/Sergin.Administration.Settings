using Sergin.Platform.Domain.Languages;

namespace Sergin.Platform.Application.Translations;

public interface ITranslationRowQueryRepository
{
    /// <summary>Every translation of <paramref name="languageId"/>, orphaned ones included.</summary>
    Task<IReadOnlyCollection<TranslationRow>> GetRowsAsync(LanguageId languageId, CancellationToken cancellationToken = default);
}
