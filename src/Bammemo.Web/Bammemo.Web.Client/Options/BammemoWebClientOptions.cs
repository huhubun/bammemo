namespace Bammemo.Web.Client.Options;

public class BammemoWebClientOptions
{
    public const string FileName = "bammemo.json";
    public const string Position = "Bammemo";

    public required string ApiUrl { get; set; }
}
