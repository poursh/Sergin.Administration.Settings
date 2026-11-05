using Sergin.SharedKernel.Domain.Repositories;

namespace Sergin.Platform.Domain.Languages;

/// <summary>
/// The culture is the alternate key, compared after normalisation (<see cref="CultureName.Normalise"/>), so
/// <c>FA-ir</c> is taken once <c>fa-IR</c> is; <c>ix_language_culture</c> is the guarantee.
/// </summary>
public interface ILanguageRepository : IRepository<Language, LanguageId>, IUniqueKeyRepository<CultureName>;
