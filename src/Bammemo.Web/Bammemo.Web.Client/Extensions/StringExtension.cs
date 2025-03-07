namespace Bammemo.Web.Client.Extensions;

public static class StringExtension
{
    public static string NormalizeUrlSlash(this string url)
        => url.EndsWith("/") ? url : url + "/";
}
