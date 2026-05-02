using Journal.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Journal.Infrastructure.Persistence.EntityConfiguration
{
    internal class DatabaseIndexationMap : IEntityTypeConfiguration<DatabaseIndexation>
    {
        public void Configure(EntityTypeBuilder<DatabaseIndexation> entity)
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("database_indexation");

            entity.Property(e => e.Id)
                  .HasColumnName("id");

            entity.Property(e => e.Description)
                .HasMaxLength(50)
                .HasColumnName("description");
        }
    }
}