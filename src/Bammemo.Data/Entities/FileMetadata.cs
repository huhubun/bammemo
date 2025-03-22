using Bammemo.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Bammemo.Data.Entities;

[EntityTypeConfiguration(typeof(FileMetadataConfiguration))]
public class FileMetadata
{
    public int Id { get; set; }
    public required string FileName { get; set; }
    public required string StorageFileName { get; set; }
    public int StorageType { get; set; }
    public int FileType { get; set; }
    public required string Path { get; set; }
    public long Size { get; set; }
    public required string HashValue { get; set; }
    public required string HashAlgorithm { get; set; }
    public long CreatedAt { get; set; }
    public long? UpdateAt { get; set; }

    public virtual ICollection<FileReference>? References { get; set; }
}
