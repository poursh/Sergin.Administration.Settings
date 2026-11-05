using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sergin.Platform.Domain.Languages;

namespace Sergin.Platform.Infrastructure.Data.Languages.Converters;

internal sealed class LanguageDisplayNameConverter : ValueConverter<LanguageDisplayName, string>
{
    private static readonly ConverterMappingHints defaultHints = new();

    public LanguageDisplayNameConverter() : this(null)
    {
    }

    public LanguageDisplayNameConverter(ConverterMappingHints? mappingHints)
        : base(
                convertToProviderExpression: x => x.Value,
                convertFromProviderExpression: x => new LanguageDisplayName(x),
                mappingHints: defaultHints.With(mappingHints))
    {
    }
}
