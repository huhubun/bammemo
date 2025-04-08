namespace Bammemo.Web.WebApiModels.Files;

public class UploadFileResponse
{
    public int FileMetadataId { get; set; }
    public required string FileName { get; set; }
    public required string Url { get; set; }
}
