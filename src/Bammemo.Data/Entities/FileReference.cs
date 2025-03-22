using Bammemo.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Bammemo.Data.Entities;

[EntityTypeConfiguration(typeof(FileReferenceConfiguration))]
public class FileReference
{
    public int Id { get; set; }
    public int MetadataId { get; set; }
    public int SourceId { get; set; }
    public int SourceType { get; set; }

    public virtual FileMetadata? Metadata { get; set; }
}
