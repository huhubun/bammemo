using Bammemo.Data.Entities;
using Bammemo.Service.Storages;

namespace Bammemo.Service.Interfaces;

public interface IStorageService
{
    IAsyncEnumerable<StorageTypeInfo> GetStorageTypesAsync();
    Task<FileMetadata> SaveAsync(string fileName, FileType fileType, Stream stream, FileReferenceSourceType? sourceType = null, int? sourceId = null);
    Task<FileDeleteResult> DeleteAsync(int fileMetadataId);
    Task<FileMetadata?> GetFileMetadataByFullName(string fullName);
    Task<FileReadResult> ReadAsync(FileMetadata fileMetadata);
    Task<List<FileMetadata>> GetFileMetadatasBySourceIdAsync(int sourceId);
    Task SaveReferencesAsync(List<FileReference> newReferences, List<FileReference> updatedReferences);
}
