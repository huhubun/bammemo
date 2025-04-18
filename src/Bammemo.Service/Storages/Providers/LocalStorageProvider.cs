﻿using Bammemo.Data.Entities;
using Bammemo.Service.Options;
using Microsoft.Extensions.Options;

namespace Bammemo.Service.Storages.Providers;

public class LocalStorageProvider(
    IOptions<BammemoOptions> bammemoOptions) : IStorageProvider
{
    public StorageType StorageType => StorageType.LocalStorage;

    public Task<StorageTypeInfo> GetStorageTypeInfoAsync()
        => Task.FromResult(new StorageTypeInfo
        {
            StorageType = StorageType.LocalStorage,
            IsAvailable = true
        });

    public async Task SaveAsync(string path, string fileName, Stream stream)
    {
        var fullPath = Path.Combine(bammemoOptions.Value.StoragePath, path, fileName);
        var tmpFileFullPath = fullPath + ".tmp";

        try
        {
            if (!File.Exists(fullPath))
            {
                var directory = Path.GetDirectoryName(fullPath);
                if (!String.IsNullOrEmpty(directory))
                {
                    Directory.CreateDirectory(directory);
                }
            }
            else
            {
                File.Move(fullPath, tmpFileFullPath);
            }

            using var fileStream = File.Create(fullPath);
            stream.Seek(0, SeekOrigin.Begin);
            await stream.CopyToAsync(fileStream);

            if (File.Exists(tmpFileFullPath))
            {
                File.Delete(tmpFileFullPath);
            }
        }
        catch (Exception ex)
        {
            // TODO Log

            if (File.Exists(tmpFileFullPath))
            {
                File.Delete(fullPath);
                File.Move(tmpFileFullPath, fullPath);
            }
        }
    }

    public Task<FileReadResult> ReadAsync(FileMetadata fileMetadata)
        => Task.FromResult(new FileReadResult
        {
            Type = FileReadResultType.Stream,
            Stream = File.OpenRead(Path.Combine(bammemoOptions.Value.StoragePath, fileMetadata.Path, fileMetadata.StorageFileName))
        });

    public Task<FileDeleteResult> DeleteAsync(string path, string fileName)
    {
        var fullPath = Path.Combine(bammemoOptions.Value.StoragePath, path, fileName);

        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
        else
        {
            // TODO LOG
        }

        return Task.FromResult(new FileDeleteResult
        {
            Status = FileDeleteStatus.Deleted
        });
    }
}
