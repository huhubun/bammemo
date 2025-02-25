namespace Bammemo.Service.Abstractions.Dtos.Settings;

public class BatchGetSettingByKeyDto
{
    public required List<SettingItemModel> Settings { get; set; }

    public class SettingItemModel
    {
        public int Id { get; set; }
        public required string Key { get; set; }
        public string? Value { get; set; }
    }
}
