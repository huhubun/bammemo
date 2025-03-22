namespace Bammemo.Web.WebApiModels.Settings;

public class BatchUpdateSettingByKeyRequest
{
    public required Dictionary<string, string?> Settings { get; set; }
}
