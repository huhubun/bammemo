namespace Bammemo.Service.Abstractions.WebApiModels.Settings;

public class BatchGetSettingByKeyResponse
{
    public required SettingItemModel[] Settings { get; set; }

    public class SettingItemModel
    {
        public int Id { get; set; }
        public required string Key { get; set; }
        public string? Value { get; set; }
    }
}

