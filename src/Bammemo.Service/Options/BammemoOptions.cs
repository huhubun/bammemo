namespace Bammemo.Service.Options;

public class BammemoOptions
{
    public const string Position = "Bammemo";

    public required string ConnectionString { get; set; }
    public required string ApiUrl { get; set; }
    public required string Username { get; set; }
    public required string Password { get; set; }
}
