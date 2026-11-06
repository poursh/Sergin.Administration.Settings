using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Sergin.Administration.Settings.Domain.Languages;

namespace Sergin.Administration.Settings.Infrastructure.Data.Languages.Converters;

internal sealed class CultureNameConverter : ValueConverter<CultureName, string>
{
    private static readonly ConverterMappingHints defaultHints = new();

    public CultureNameConverter() : this(null)
    {
    }

    public CultureNameConverter(ConverterMappingHints? mappingHints)
        : base(
                convertToProviderExpression: x => x.Value,
                convertFromProviderExpression: x => new CultureName(x),
                mappingHints: defaultHints.With(mappingHints))
    {
    }
}
