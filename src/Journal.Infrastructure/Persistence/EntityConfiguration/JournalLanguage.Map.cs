using Journal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Journal.Infrastructure.Persistence.EntityConfiguration
{
    public class JournalLanguageMap : IEntityTypeConfiguration<JournalLanguage>
    {
        public void Configure(EntityTypeBuilder<JournalLanguage> builder)
        {
            builder
                .HasNoKey()
                    .ToTable("journal_language");

            builder.HasIndex(e => e.Journalid, "journalid");

            builder.HasIndex(e => e.Languageid, "languageid");

            builder.Property(e => e.Journalid).HasColumnName("journalid");
            
            builder.Property(e => e.Languageid).HasColumnName("languageid");

            builder.HasOne(d => d.Journal).WithMany()
                    .HasForeignKey(d => d.Journalid)
                    .HasConstraintName("journal_language_ibfk_1");

            builder.HasOne(d => d.Language).WithMany()
                    .HasForeignKey(d => d.Languageid)
                    .HasConstraintName("journal_language_ibfk_2");
        }
    }
}