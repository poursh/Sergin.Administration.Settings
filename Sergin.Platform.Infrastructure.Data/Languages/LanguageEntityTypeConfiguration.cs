using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sergin.Platform.Domain.Languages;
using Sergin.Platform.Infrastructure.Data.Languages.Converters;

namespace Sergin.Platform.Infrastructure.Data.Languages;

internal sealed class LanguageEntityTypeConfiguration : IEntityTypeConfiguration<Language>
{
    // Wide enough for the longest enum member name; the enums are stored by name so a row reads in psql.
    private const int EnumMaxLength = 20;

    public void Configure(EntityTypeBuilder<Language> builder)
    {
        builder.HasKey(l => l.Id);

        builder.Property(l => l.Id)
            .HasConversion<LanguageIdConverter>()
            .ValueGeneratedNever();

        builder.Property(l => l.Culture)
            .HasConversion<CultureNameConverter>()
            .HasMaxLength(CultureName.MaxLength)
            .IsRequired();

        builder.Property(l => l.DisplayName)
            .HasConversion<LanguageDisplayNameConverter>()
            .HasMaxLength(LanguageDisplayName.MaxLength)
            .IsRequired();

        builder.Property(l => l.Direction)
            .HasConversion<string>()
            .HasMaxLength(EnumMaxLength)
            .IsRequired();

        builder.Property(l => l.Status)
            .HasConversion<string>()
            .HasMaxLength(EnumMaxLength)
            .IsRequired();

        // Derived from Culture and Direction; never stored.
        builder.Ignore(l => l.IsRightToLeft);

        // Backs ILanguageRepository's IUniqueKeyRepository<CultureName>. Not partial: Language is not soft-deletable.
        builder.HasIndex(l => l.Culture)
            .IsUnique()
            .HasDatabaseName("ix_language_culture");
    }
}
