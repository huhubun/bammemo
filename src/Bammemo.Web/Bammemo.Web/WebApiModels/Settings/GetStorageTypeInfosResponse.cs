namespace Bammemo.Web.WebApiModels.Settings;

public class GetStorageTypeInfosResponse
{
    public required StorageTypeInfoModel[] StorageTypeInfos { get; set; }

    public class StorageTypeInfoModel
    {
        public StorageType StorageType { get; set; }
        public bool IsUsed{ get; set; }
        public bool IsAvailable { get; set; }
        public StorageTypeErrorType? Error { get; set; }
    }
}
