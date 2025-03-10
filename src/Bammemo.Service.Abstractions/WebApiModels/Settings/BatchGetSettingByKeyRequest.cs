namespace Bammemo.Service.Abstractions.WebApiModels.Settings;

public class BatchGetSettingByKeyRequest
{
    public required string[] Keys { get; set; }
}
