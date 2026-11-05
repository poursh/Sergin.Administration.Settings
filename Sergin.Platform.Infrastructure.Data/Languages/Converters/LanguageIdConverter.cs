using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sergin.Platform.Domain.Languages;

namespace Sergin.Platform.Infrastructure.Data.Languages.Converters;

internal sealed class LanguageIdConverter : ValueConverter<LanguageId, Guid>
{
    private static readonly ConverterMappingHints defaultHints = new();

    public LanguageIdConverter() : this(null)
    {
    }

    public LanguageIdConverter(ConverterMappingHints? mappingHints)
        : base(
                convertToProviderExpression: x => x.Value,
                convertFromProviderExpression: x => new LanguageId(x),
                mappingHints: defaultHints.With(mappingHints))
    {
    }
}
