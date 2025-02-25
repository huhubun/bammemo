namespace Bammemo.Service.Abstractions.WebApiModels.Settings;

public class BatchGetSettingByKeyRequest
{
    public required IEnumerable<string> Keys { get; set; }
}
