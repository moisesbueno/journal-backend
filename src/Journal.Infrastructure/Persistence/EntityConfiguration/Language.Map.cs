using Journal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Journal.Infrastructure.Persistence.EntityConfiguration
{
    public class LanguageMap : IEntityTypeConfiguration<Language>
    {
        public void Configure(EntityTypeBuilder<Language> builder)
        {
            builder.HasKey(e => e.Id).HasName("PRIMARY");

            builder.ToTable("language");

            builder.Property(e => e.Id)
                    .HasColumnType("char(36)")
                    .HasColumnName("id");

            builder.Property(e => e.Description)
                    .HasMaxLength(50)
                    .HasColumnName("description");
        }
    }
}