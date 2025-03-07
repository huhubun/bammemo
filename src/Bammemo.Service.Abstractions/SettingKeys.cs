namespace Bammemo.Service.Abstractions;

public static class SettingKeys
{
    public const string IdAlphabet = nameof(IdAlphabet);
    public const string SiteName = nameof(SiteName);
    public const string SiteLogoText = nameof(SiteLogoText);
    public const string SiteDescription= nameof(SiteDescription);
    public const string SiteKeywords= nameof(SiteKeywords);
    public const string FooterLinks = nameof(FooterLinks);

    public static bool VerifyKey(string key)
        => VerifyKeys([key]);

    public static bool VerifyKeys(IEnumerable<string> keys)
        => TryVerifyKeys(keys, out _);

    public static bool TryVerifyKeys(IEnumerable<string> keys, out string[] wrongKeys)
    {
        wrongKeys = [.. keys.Except(typeof(SettingKeys).GetFields().Where(f => f.IsPublic).Select(f => f.GetValue(null)).Cast<string>())];
        return wrongKeys.Length == 0;
    }
}
