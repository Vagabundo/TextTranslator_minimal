using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MinimalTranslator.Domain.Translations;

namespace MinimalTranslator.Database.Configurations;

internal sealed class TranslationConfiguration : IEntityTypeConfiguration<Translation>
{
    public void Configure(EntityTypeBuilder<Translation> builder)
    {
        builder.ToTable("Translations");

        //specific for Postgresql
        builder.Property<Guid>("Id")
            .HasColumnType("uuid");

        builder.HasKey(t => new { t.Id, t.LanguageTo });

        builder.Property(t => t.OriginalText)
            .HasMaxLength(400)
            .HasConversion(text => text!.Value, value => new Text(value));

        builder.Property(t => t.LanguageFrom)
            .HasMaxLength(10)
            .HasConversion(language => language!.Value, value => new Language(value));

        builder.Property(t => t.TranslatedText)
            .HasMaxLength(400)
            .HasConversion(text => text!.Value, value => new Text(value));

        builder.Property(t => t.LanguageTo)
            .HasMaxLength(10)
            .HasConversion(language => language!.Value, value => new Language(value));
    }
}
