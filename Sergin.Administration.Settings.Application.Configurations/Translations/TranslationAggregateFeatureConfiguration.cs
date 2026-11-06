using Sergin.Administration.Settings.Domain.Translations;
using Sergin.SharedKernel.Application.Aggregates;

namespace Sergin.Administration.Settings.Application.Configurations.Translations;

// Not SoftDeletable: a deleted translation frees its key at once. Not Scoped: translations are the whole platform's.
internal sealed class TranslationAggregateFeatureConfiguration : IAggregateFeatureConfiguration<Translation>
{
    public void Configure(AggregateFeatureBuilder<Translation> builder) => builder.Audited().Versioned()
            .Permissions(TranslationPermissions.Edit)
            .Texts(TranslationTexts.Category)
            .Named(TranslationTexts.Singular);
}
