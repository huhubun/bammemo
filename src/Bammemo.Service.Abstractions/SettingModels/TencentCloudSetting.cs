namespace Bammemo.Service.Abstractions.SettingModels;

public class TencentCloudSetting : SettingBase
{
    public string? AppId { get; set; }
    public string? SecretId { get; set; }
    public string? SecretKey { get; set; }

    public CosSetting? Cos { get; set; }

    public class CosSetting
    {
        public string? Region { get; set; }
        public string? Bucket { get; set; }
    }
}