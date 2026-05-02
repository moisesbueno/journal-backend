using Journal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Journal.Infrastructure.Persistence.EntityConfiguration
{
    public class ImportacaoMap : IEntityTypeConfiguration<Importacao>
    {
        public void Configure(EntityTypeBuilder<Importacao> builder)
        {
            builder
                .HasNoKey()
                    .ToTable("importacao");

            builder.Property(e => e.Issn)
                    .HasMaxLength(20)
                    .HasColumnName("issn");

            builder.Property(e => e.Name)
                    .HasMaxLength(255)
                    .HasColumnName("name");

            builder.Property(e => e.Qualis2019)
                    .HasMaxLength(10)
                    .HasColumnName("qualis_2019");
        }
    }
}