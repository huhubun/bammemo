using Bammemo.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bammemo.Data.Configurations;

public class FileReferenceConfiguration : IEntityTypeConfiguration<FileReference>
{
    public void Configure(EntityTypeBuilder<FileReference> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id).ValueGeneratedOnAdd();

        builder.HasOne(st => st.Metadata).WithMany(s => s.References).HasForeignKey(st => st.MetadataId);

        builder.HasIndex(s => s.MetadataId);
        builder.HasIndex(s => new { s.SourceType, s.SourceId });
    }
}
