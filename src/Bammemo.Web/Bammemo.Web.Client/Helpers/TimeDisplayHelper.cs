using Microsoft.FluentUI.AspNetCore.Components.Extensions;

namespace Bammemo.Web.Client.Helpers;

public static class TimeDisplayHelper
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="ticks">UTC time ticks</param>
    /// <returns></returns>
    public static string ToLocalTimeString(this long ticks)
        => new DateTime(ticks, DateTimeKind.Utc).ToLocalTime().ToString("yyyy-MM-dd HH:mm");

    /// <summary>
    /// 
    /// </summary>
    /// <param name="ticks">UTC time ticks</param>
    /// <returns></returns>
    public static string GetTimeAgo(this long ticks)
    {
        var timeSpan = new TimeSpan(DateTime.UtcNow.Ticks - ticks);
        return timeSpan.ToTimeAgo();
    }
}
