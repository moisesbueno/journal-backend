using Journal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Journal.Infrastructure.Persistence.EntityConfiguration
{
    public class JournalIndexationMap : IEntityTypeConfiguration<JournalIndexation>
    {
        public void Configure(EntityTypeBuilder<JournalIndexation> builder)
        {
            builder
                .HasNoKey()
                    .ToTable("journal_indexation");

            builder.HasIndex(e => e.Journalid, "journalid");

            builder.HasIndex(e => e.Journalindexationid, "journalindexationid");

            builder.Property(e => e.Journalid).HasColumnName("journalid");
            
            builder.Property(e => e.Journalindexationid).HasColumnName("journalindexationid");

            builder.HasOne(d => d.Journal).WithMany()
                    .HasForeignKey(d => d.Journalid);

            builder.HasOne(d => d.Journalindexation).WithMany()
                    .HasForeignKey(d => d.Journalindexationid);
        }
    }
}