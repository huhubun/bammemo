namespace Bammemo.Service.Options;

public class BammemoOptions
{
    public const string Position = "Bammemo";

    public required string ConnectionString { get; set; }
    public required string ApiUrl { get; set; }
    public string? WebUrl { get; set; }

    public string ApiUrlAuthority => GetUrlAuthority(ApiUrl);
    public string? WebUrlAuthority => WebUrl != null ? GetUrlAuthority(ApiUrl) : null;

    private static string GetUrlAuthority(string url)
        => new Uri(url).GetLeftPart(UriPartial.Authority);
}
