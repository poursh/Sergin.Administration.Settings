using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sergin.Administration.Settings.Domain.Languages;
using Sergin.Administration.Settings.Domain.Translations;
using Sergin.Administration.Settings.Infrastructure.Data.Languages.Converters;
using Sergin.Administration.Settings.Infrastructure.Data.Translations.Converters;

namespace Sergin.Administration.Settings.Infrastructure.Data.Translations;

internal sealed class TranslationEntityTypeConfiguration : IEntityTypeConfiguration<Translation>
{
    public void Configure(EntityTypeBuilder<Translation> builder)
    {
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasConversion<TranslationIdConverter>()
            .ValueGeneratedNever();

        builder.Property(t => t.LanguageId)
            .HasConversion<LanguageIdConverter>()
            .IsRequired();

        builder.Property(t => t.Key)
            .HasConversion<TranslationKeyConverter>()
            .HasMaxLength(TranslationKey.MaxLength)
            .IsRequired();

        builder.Property(t => t.Text)
            .HasConversion<TranslationTextConverter>()
            .HasMaxLength(TranslationText.MaxLength)
            .IsRequired();

        // A default's length is the code's; text, unbounded.
        builder.Property(t => t.SourceText)
            .HasConversion<SourceTextConverter>()
            .IsRequired();

        // Deleting a language deletes its translations in the same statement: the database cascades, so none has to be
        // loaded. Language and Translation are separate aggregates, so there is no navigation either way.
        builder.HasOne<Language>()
            .WithMany()
            .HasForeignKey(t => t.LanguageId)
            .OnDelete(DeleteBehavior.Cascade);

        // One translation per key in a language; the guarantee behind AddTranslationCommandValidator's check.
        builder.HasIndex(t => new { t.LanguageId, t.Key })
            .IsUnique()
            .HasDatabaseName("ix_translation_language_id_key");
    }
}
