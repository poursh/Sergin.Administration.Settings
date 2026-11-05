using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sergin.Platform.Domain.Translations;

namespace Sergin.Platform.Infrastructure.Data.Translations.Converters;

internal sealed class TranslationTextConverter : ValueConverter<TranslationText, string>
{
    private static readonly ConverterMappingHints defaultHints = new();

    public TranslationTextConverter() : this(null)
    {
    }

    public TranslationTextConverter(ConverterMappingHints? mappingHints)
        : base(
                convertToProviderExpression: x => x.Value,
                convertFromProviderExpression: x => new TranslationText(x),
                mappingHints: defaultHints.With(mappingHints))
    {
    }
}
