using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sergin.Administration.Settings.Domain.Translations;

namespace Sergin.Administration.Settings.Infrastructure.Data.Translations.Converters;

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
