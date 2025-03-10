namespace Bammemo.Web.Client.Services;

public partial class WebApiClient(HttpClient httpClient)
{
    public SlipClient Slips { get; } = new SlipClient(httpClient);
    public AnalyticsClient Analytics { get; } = new AnalyticsClient(httpClient);
    public SettingClient Settings { get; } = new SettingClient(httpClient);
    public RedirectRuleClient RedirectRules { get; } = new RedirectRuleClient(httpClient);
    public SiteLinkClient SiteLinks { get; } = new SiteLinkClient(httpClient);
}
