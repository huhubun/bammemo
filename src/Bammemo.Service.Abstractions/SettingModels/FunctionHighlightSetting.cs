namespace Bammemo.Service.Abstractions.SettingModels;

public class FunctionHighlightSetting : SettingBase
{
    public string? HighlightCssLightUrl { get; set; }
    public string? HighlightCssDarkUrl { get; set; }
    public string? HighlightJsUrl { get; set; }
}
