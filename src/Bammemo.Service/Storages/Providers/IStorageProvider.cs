using Bammemo.Data.Entities;

namespace Bammemo.Service.Storages.Providers;

public interface IStorageProvider
{
    StorageType StorageType { get; }

    Task<StorageTypeInfo> GetStorageTypeInfoAsync();
    Task<FileReadResult> ReadAsync(FileMetadata fileMetadata);
    Task SaveAsync(string path, string fileName, Stream stream);
    Task<FileDeleteResult> DeleteAsync(string path, string fileName);
}
