using Bammemo.Service.Abstractions.Attributes;
using Bammemo.Service.Abstractions.Dtos.Settings;
using Bammemo.Service.Abstractions.SettingModels;
using Bammemo.Web.Client.Extensions.SettingModels;
using Bammemo.Web.Client.Models.Settings;
using Bammemo.Web.Client.WebApis.Client.Models;

namespace Bammemo.Web.Client.MapperProfiles;

[Map<GetSettingByKeyResponse, GetSettingByKeyDto>]
[Map<BatchGetSettingByKeyResponse, BatchGetSettingByKeyDto>]
[Map<SettingItemModel, BatchGetSettingByKeyDto.SettingItemModel>]
[Map<TencentCloudSetting, TencentCloudSettingModel>]
[Map<TencentCloudSetting.CosSetting, TencentCloudSettingModel.CosSettingModel>]
[Map<TencentCloudSettingModel, TencentCloudSetting>]
[Map<TencentCloudSettingModel.CosSettingModel, TencentCloudSetting.CosSetting>]
public static partial class SettingProfile
{
    static partial void AfterMap(TencentCloudSetting source, TencentCloudSettingModel target)
    {
        if (target.Cos.IsNullOrWhiteSpace())
        {
            target.Cos = null;
            target.EnableCos = false;
        }
        else
        {
            target.EnableCos = true;
        }
    }

    static partial void AfterMap(TencentCloudSettingModel source, TencentCloudSetting target)
    {
        if (!source.EnableCos)
        {
            target.Cos = null;
        }
    }
}
