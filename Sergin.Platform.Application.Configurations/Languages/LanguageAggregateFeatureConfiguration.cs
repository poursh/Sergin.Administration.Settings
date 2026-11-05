using Sergin.Platform.Domain.Languages;
using Sergin.SharedKernel.Application.Aggregates;

namespace Sergin.Platform.Application.Configurations.Languages;

// Not SoftDeletable: a deleted language frees its culture at once, and its translations go with it. Not Scoped: the
// languages are the whole platform's.
internal sealed class LanguageAggregateFeatureConfiguration : IAggregateFeatureConfiguration<Language>
{
    public void Configure(AggregateFeatureBuilder<Language> builder) => builder.Audited().Versioned()
            .Permissions(LanguagePermissions.Read, LanguagePermissions.Manage)
            .Texts(LanguageTexts.Category)
            .Named(LanguageTexts.Singular);
}
