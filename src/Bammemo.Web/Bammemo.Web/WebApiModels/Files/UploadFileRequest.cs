namespace Bammemo.Web.WebApiModels.Files;

public class UploadFileRequest
{
    public IFormFile? File { get; set; }
    public FileType Type { get; set; }
}
