using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sergin.Platform.Domain.Translations;

namespace Sergin.Platform.Infrastructure.Data.Translations.Converters;

internal sealed class SourceTextConverter : ValueConverter<SourceText, string>
{
    private static readonly ConverterMappingHints defaultHints = new();

    public SourceTextConverter() : this(null)
    {
    }

    public SourceTextConverter(ConverterMappingHints? mappingHints)
        : base(
                convertToProviderExpression: x => x.Value,
                convertFromProviderExpression: x => new SourceText(x),
                mappingHints: defaultHints.With(mappingHints))
    {
    }
}
