using System.Collections.ObjectModel;
using System.Net;

namespace Bammemo.Web.Client.Helpers;

public static class RedirectRuleHelper
{
    private static readonly ReadOnlyDictionary<int, string> _validHttpStatus = new(new Dictionary<int, string>
    {
        {(int)HttpStatusCode.Moved,  "301 Moved Permanently"},
        {(int)HttpStatusCode.Redirect,  "302 Redirect"},
    });

    public static ReadOnlyDictionary<int, string> GetValidHttpStatus()
        => _validHttpStatus;

    public static string GetHttpStatusName(int statusCode)
        => _validHttpStatus.TryGetValue(statusCode, out string? value) ? value : throw new KeyNotFoundException(statusCode.ToString());
}
