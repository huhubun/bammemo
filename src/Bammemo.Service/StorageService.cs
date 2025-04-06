using Bammemo.Data;
using Bammemo.Data.Entities;
using Bammemo.Service.Helpers;
using Bammemo.Service.Interfaces;
using Bammemo.Service.Storages;
using Bammemo.Service.Storages.Providers;
using Microsoft.EntityFrameworkCore;

namespace Bammemo.Service;

public class StorageService(
    IEnumerable<IStorageProvider> storageProviders,
    ISettingService settingService,
    BammemoDbContext dbContext) : IStorageService
{
    public async IAsyncEnumerable<StorageTypeInfo> GetStorageTypesAsync()
    {
        var currentStorageType = await GetStorageTypeAsync();

        foreach (var storageProvider in storageProviders)
        {
            var storageType = await storageProvider.GetStorageTypeInfoAsync();
            storageType.IsUsed = storageType.StorageType == currentStorageType;

            yield return storageType;
        }
    }

    public async Task<StorageType> GetStorageTypeAsync()
    {
        var setting = await settingService.GetByKeyFromCacheAsync(SettingKeys.StorageType);
        if (setting == null || setting.Value == null)
        {
            return StorageType.LocalStorage;
        }

        return Enum.Parse<StorageType>(setting.Value);
    }

    public async Task<IStorageProvider> GetStorageProviderAsync(StorageType? storageType = null)
    {
        var targetType = storageType ?? await GetStorageTypeAsync();
        return storageProviders.Single(p => p.StorageType == targetType);
    }

    public async Task<FileMetadata> SaveAsync(
        string fileName,
        FileType fileType,
        Stream stream,
        bool keepFileName = true,
        FileReferenceSourceType? sourceType = null,
        int? sourceId = null)
    {
        var path = GetPath(fileType);
        var extension = Path.GetExtension(fileName);
        var (algorithm, hash) = HashHelper.Sha256(stream);

        var storageType = await GetStorageTypeAsync();
        var provider = await GetStorageProviderAsync(storageType);
        FileMetadata? fileMetadata;

        if (keepFileName)
        {
            fileMetadata = await dbContext.FileMetadata
                .Where(f => f.Path == path && f.FileName == fileName)
                .Include(f => f.References)
                .SingleOrDefaultAsync();
        }
        else
        {
            fileName = $"{Guid.NewGuid():n}{extension}";
            fileMetadata = null;
        }

        if (fileMetadata == null)
        {
            fileMetadata = new FileMetadata
            {
                FileName = fileName,
                // 避免实际保存使用的文件名被猜测
                StorageFileName = $"{Guid.NewGuid():n}{extension}",
                Path = path,
                Size = stream.Length,
                FileType = (int)fileType,
                HashAlgorithm = algorithm,
                HashValue = hash,
                StorageType = (int)storageType,
                CreatedAt = DateTime.UtcNow.Ticks
            };

            dbContext.FileMetadata.Add(fileMetadata);
        }
        else
        {
            fileMetadata.StorageFileName = $"{Guid.NewGuid():n}{extension}";
            fileMetadata.Size = stream.Length;
            fileMetadata.HashAlgorithm = algorithm;
            fileMetadata.HashValue = hash;
            fileMetadata.StorageType = (int)storageType;
            fileMetadata.CreatedAt = DateTime.UtcNow.Ticks;
        }

        await provider.SaveAsync(path, fileMetadata.StorageFileName, stream);

        if (sourceId != null)
        {
            fileMetadata.References ??= [];

            if (!fileMetadata.References.Any(r => r.SourceType == (int)sourceType && r.SourceId == sourceId.Value))
            {
                fileMetadata.References.Add(new()
                {
                    SourceType = (int)sourceType,
                    SourceId = sourceId.Value
                });
            }
        }

        await dbContext.SaveChangesAsync();

        return fileMetadata;
    }

    public async Task<FileReadResult> ReadAsync(FileMetadata fileMetadata)
    {
        var provider = await GetStorageProviderAsync((StorageType)fileMetadata.StorageType);
        return await provider.ReadAsync(fileMetadata);
    }

    public async Task DeleteAsync()
    {

    }

    public async Task<FileMetadata?> GetFileMetadataByFullName(string fullName)
    {
        var (path, fileName) = ParseFilePath(fullName);

        return await dbContext.FileMetadata.AsNoTracking()
            .Where(f => f.Path == path && f.FileName == fileName)
            .Include(f => f.References)
            .SingleOrDefaultAsync();
    }

    public async Task SaveReferencesAsync(IEnumerable<FileReference> references)
    {
        if (references.Any())
        {
            await dbContext.FileReferences.AddRangeAsync(references);
            await dbContext.SaveChangesAsync();
        }
        else
        {
            // TODO LOG
        }
    }

    private static string GetPath(FileType fileType) => fileType switch
    {
        FileType.Favicon => "favicon",
        FileType.SiteLogo => "logo",
        FileType.SlipAttachment => $"slip_attachment/{DateTime.UtcNow.Year}/{DateTime.UtcNow.Month}/{DateTime.UtcNow.Day}",
        _ => throw new NotSupportedException(fileType.ToString())
    };

    private static (string path, string fileName) ParseFilePath(string fullName)
        => (
        (Path.GetDirectoryName(fullName) ?? throw new NullReferenceException("path")).Replace('\\', '/'),
        Path.GetFileName(fullName));
}
