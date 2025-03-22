namespace Bammemo.Web.WebApiModels.Settings;

public class BatchGetSettingByKeyRequest
{
    public required string[] Keys { get; set; }
}
