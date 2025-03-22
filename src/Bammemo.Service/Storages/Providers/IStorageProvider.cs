using Bammemo.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bammemo.Service.Storages.Providers;

public interface IStorageProvider
{
    StorageType StorageType { get; }

    Task<StorageTypeInfo> GetStorageTypeInfoAsync();
    Task<FileReadResult> ReadAsync(FileMetadata fileMetadata);
    Task SaveAsync(string path, string fileName, Stream stream);
}
