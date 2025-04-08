using System.Web;

namespace Bammemo.Web.Client.Helpers;

public static class UrlHelper
{
    public static Uri AppendQueryString(string url, params IEnumerable<KeyValuePair<string, string>> parameters)
    {
        var uriBuilder = new UriBuilder(url);
        var queryParams = HttpUtility.ParseQueryString(uriBuilder.Query);

        foreach (var kvp in parameters)
        {
            queryParams.Add(kvp.Key, kvp.Value);
        }

        uriBuilder.Query = queryParams.ToString();

        return uriBuilder.Uri;
    }
}
