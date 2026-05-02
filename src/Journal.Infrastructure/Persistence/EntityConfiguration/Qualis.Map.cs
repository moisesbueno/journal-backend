using Journal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Journal.Infrastructure.Persistence.EntityConfiguration
{
    public class QualisMap : IEntityTypeConfiguration<Qualis>
    {
        public void Configure(EntityTypeBuilder<Qualis> builder)
        {
            builder.HasKey(e => e.Id).HasName("PRIMARY");

            builder.ToTable("qualis");

            builder.Property(e => e.Id)
                    .HasColumnType("char(36)")
                    .HasColumnName("id");

            builder.Property(e => e.Description)
                    .HasMaxLength(10)
                    .HasColumnName("description");
        }
    }
}