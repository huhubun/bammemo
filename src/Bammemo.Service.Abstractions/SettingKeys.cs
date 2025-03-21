using Bammemo.Service.Abstractions.Attributes;
using System.Reflection;

namespace Bammemo.Service.Abstractions;

public static class SettingKeys
{
    public const string IdAlphabet = nameof(IdAlphabet);
    public const string SiteName = nameof(SiteName);
    public const string SiteLogoText = nameof(SiteLogoText);
    public const string SiteDescription = nameof(SiteDescription);
    public const string SiteKeywords = nameof(SiteKeywords);
    public const string FooterLinks = nameof(FooterLinks);
    public const string AboutPageDescription = nameof(AboutPageDescription);
    public const string AboutPageKeywords = nameof(AboutPageKeywords);
    public const string AboutPageContent = nameof(AboutPageContent);
    public const string FaviconUrl = nameof(FaviconUrl);
    [ProtectedSetting]
    public const string TencentCloud = nameof(TencentCloud);

    public static bool VerifyKey(string key)
        => VerifyKeys([key]);

    public static bool VerifyKeys(IEnumerable<string> keys)
        => TryVerifyKeys(keys, out _);

    public static bool TryVerifyKeys(IEnumerable<string> keys, out string[] wrongKeys)
    {
        wrongKeys = [.. keys.Except(typeof(SettingKeys).GetFields().Where(f => f.IsPublic).Select(f => f.Name))];
        return wrongKeys.Length == 0;
    }

    public static bool CheckProtectedSettingByKey(string key)
        => CheckProtectedSettingByKeys([key]);

    public static bool CheckProtectedSettingByKeys(IEnumerable<string> keys)
        => typeof(SettingKeys).GetFields().Where(f => f.IsPublic).Where(f => keys.Contains(f.Name)).Any(f => f.GetCustomAttribute<ProtectedSettingAttribute>() != null);
}
