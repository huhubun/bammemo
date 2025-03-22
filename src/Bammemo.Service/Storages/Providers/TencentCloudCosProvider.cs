using Bammemo.Data.Entities;
using Bammemo.Service.Abstractions.SettingModels;
using Bammemo.Service.Interfaces;
using Bammemo.Service.Options;
using COSXML;
using COSXML.Auth;
using COSXML.Transfer;
using Microsoft.Extensions.Options;
using System.Text.Json;

namespace Bammemo.Service.Storages.Providers;

public class TencentCloudCosProvider(
    IOptions<BammemoOptions> bammemoOptions,
    ISettingService settingService) : IStorageProvider
{
    private const string LOCAL_TEMP_FOLDER_PATH = "tmp/TencentCloudCos";
    private const string COS_PREFIX= "bammemo";

    public StorageType StorageType => StorageType.TencentCloudCos;

    public async Task<StorageTypeInfo> GetStorageTypeInfoAsync()
    {
        var tencentCloudCos = new StorageTypeInfo
        {
            StorageType = StorageType.TencentCloudCos,
            IsAvailable = false
        };

        var settingEntity = await settingService.GetByKeyAsync(SettingKeys.TencentCloud);
        if (settingEntity == null || settingEntity.Value == null)
        {
            tencentCloudCos.Error = StorageTypeErrorType.NotConfigured;
        }
        else
        {
            var tencentCloudSetting = await GetTencentCloudSettingAsync();
            if (tencentCloudSetting == null || tencentCloudSetting.Cos == null)
            {
                tencentCloudCos.Error = StorageTypeErrorType.NotConfigured;
            }
            else
            {
                tencentCloudCos.IsAvailable = true;
            }
        }

        return tencentCloudCos;
    }

    public async Task<FileReadResult> ReadAsync(FileMetadata fileMetadata)
    {
        var tencentCloudSetting = await GetTencentCloudSettingAsync();

        if (tencentCloudSetting?.Cos == null)
        {
            throw new OptionsValidationException(SettingKeys.TencentCloud, typeof(TencentCloudSetting), ["COS not configured"]);
        }

        return new FileReadResult
        {
            Type = FileReadResultType.Url,
            Url = $"{tencentCloudSetting.Cos.AccessDomain}/{COS_PREFIX}/{fileMetadata.Path}/{fileMetadata.StorageFileName}"
        };
    }

    public async Task SaveAsync(string path, string fileName, Stream stream)
    {
        var tencentCloudSetting = await GetTencentCloudSettingAsync();

        if (tencentCloudSetting?.Cos == null)
        {
            throw new OptionsValidationException(SettingKeys.TencentCloud, typeof(TencentCloudSetting), ["COS not configured"]);
        }

        var config = new CosXmlConfig.Builder()
          .IsHttps(true)
          .SetRegion(tencentCloudSetting.Cos.Region)
          .SetDebugLog(true)
          .Build();

        var cosCredentialProvider = new DefaultQCloudCredentialProvider(
          tencentCloudSetting.SecretId,
          tencentCloudSetting.SecretKey,
          //每次请求签名有效时长，单位为秒
          600);

        var cosXml = new CosXmlServer(config, cosCredentialProvider);
        var transferConfig = new TransferConfig();
        var transferManager = new TransferManager(cosXml, transferConfig);

        var tempFilePath = Path.Combine(bammemoOptions.Value.StoragePath, LOCAL_TEMP_FOLDER_PATH, path, fileName);
        var directory = Path.GetDirectoryName(tempFilePath);

        if (Path.Exists(tempFilePath))
        {
            File.Delete(tempFilePath);
        }
        else if (!String.IsNullOrEmpty(directory))
        {
            Directory.CreateDirectory(directory);
        }

        using (var localTempFile = File.Create(tempFilePath))
        {
            stream.Seek(0, SeekOrigin.Begin);
            await stream.CopyToAsync(localTempFile);
        }

        var uploadTask = new COSXMLUploadTask(tencentCloudSetting.Cos.Bucket, $"{COS_PREFIX}/{path}/{fileName}");
        uploadTask.SetSrcPath(tempFilePath);

        uploadTask.progressCallback = delegate (long completed, long total)
        {
            Console.WriteLine(String.Format("progress = {0:##.##}%", completed * 100.0 / total));
        };

        try
        {
            COSXML.Transfer.COSXMLUploadTask.UploadTaskResult result = await transferManager.UploadAsync(uploadTask);
            Console.WriteLine(result.GetResultInfo());
            string eTag = result.eTag;
        }
        catch (Exception e)
        {
            Console.WriteLine("CosException: " + e);
        }
        finally
        {
            if (Path.Exists(tempFilePath))
            {
                File.Delete(tempFilePath);
            }
        }
    }

    private async Task<TencentCloudSetting?> GetTencentCloudSettingAsync()
    {
        var settingEntity = await settingService.GetByKeyAsync(SettingKeys.TencentCloud);

        if (settingEntity == null || settingEntity.Value == null)
        {
            return null;
        }

        return JsonSerializer.Deserialize<TencentCloudSetting>(settingEntity.Value, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
    }
}
