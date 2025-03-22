using Bammemo.Web.Client.Models.Settings;

namespace Bammemo.Web.Client.Extensions.SettingModels;

public static class TencentCloudSettingModelExtension
{
    public static bool IsNullOrWhiteSpace(this TencentCloudSettingModel.CosSettingModel? cosSetting)
        => cosSetting == null || String.IsNullOrWhiteSpace(cosSetting.Region) || String.IsNullOrWhiteSpace(cosSetting.Bucket) || String.IsNullOrWhiteSpace(cosSetting.AccessDomain);
}
