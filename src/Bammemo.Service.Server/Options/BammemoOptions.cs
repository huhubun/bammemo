namespace Bammemo.Service.Server.Configurations;

public class BammemoOptions
{
    public const string Position = "Bammemo";

    public required string ConnectionString { get; set; }
    public required string ApiUrl { get; set; }
    public string? WebUrl { get; set; }
}
