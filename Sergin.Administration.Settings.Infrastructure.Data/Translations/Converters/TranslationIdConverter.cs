using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sergin.Administration.Settings.Domain.Translations;

namespace Sergin.Administration.Settings.Infrastructure.Data.Translations.Converters;

internal sealed class TranslationIdConverter : ValueConverter<TranslationId, Guid>
{
    private static readonly ConverterMappingHints defaultHints = new();

    public TranslationIdConverter() : this(null)
    {
    }

    public TranslationIdConverter(ConverterMappingHints? mappingHints)
        : base(
                convertToProviderExpression: x => x.Value,
                convertFromProviderExpression: x => new TranslationId(x),
                mappingHints: defaultHints.With(mappingHints))
    {
    }
}
