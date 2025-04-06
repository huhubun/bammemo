using Bammemo.Data.Entities;
using Bammemo.Service.Storages;

namespace Bammemo.Service.Interfaces;

public interface IStorageService
{
    IAsyncEnumerable<StorageTypeInfo> GetStorageTypesAsync();
    Task<FileMetadata> SaveAsync(string fileName, FileType fileType, Stream stream, bool keepFileName = true, FileReferenceSourceType? sourceType = null, int? sourceId = null);
    Task<FileMetadata?> GetFileMetadataByFullName(string fullName);
    Task<FileReadResult> ReadAsync(FileMetadata fileMetadata);
    Task SaveReferencesAsync(IEnumerable<FileReference> references);
}
