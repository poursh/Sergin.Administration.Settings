using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sergin.Platform.Domain.Translations;

namespace Sergin.Platform.Infrastructure.Data.Translations.Converters;

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
