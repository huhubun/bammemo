namespace Bammemo.Service.Storages;

public class StorageTypeInfo
{
    public StorageType StorageType { get; set; }
    public bool IsUsed { get; set; }
    public bool IsAvailable { get; set; }
    public StorageTypeErrorType? Error { get; set; }
}
