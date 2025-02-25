namespace Bammemo.Service.Abstractions.SettingModels
{
    public class TextUrlSetting : SettingBase
    {
        public required string Text { get; set; }
        public string? Url { get; set; }
    }
}
