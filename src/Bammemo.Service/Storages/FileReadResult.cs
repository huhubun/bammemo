namespace Bammemo.Service.Storages;

public class FileReadResult
{
    public FileReadResultType Type { get; set; }
    public string? Url { get; set; }
    public Stream? Stream { get; set; }
}
