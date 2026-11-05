using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sergin.Platform.Domain.Translations;

namespace Sergin.Platform.Infrastructure.Data.Translations.Converters;

internal sealed class TranslationKeyConverter : ValueConverter<TranslationKey, string>
{
    private static readonly ConverterMappingHints defaultHints = new();

    public TranslationKeyConverter() : this(null)
    {
    }

    public TranslationKeyConverter(ConverterMappingHints? mappingHints)
        : base(
                convertToProviderExpression: x => x.Value,
                convertFromProviderExpression: x => new TranslationKey(x),
                mappingHints: defaultHints.With(mappingHints))
    {
    }
}
