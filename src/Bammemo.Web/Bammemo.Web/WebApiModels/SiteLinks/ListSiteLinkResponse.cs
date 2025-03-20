namespace Bammemo.Web.WebApiModels.SiteLinks;

public class ListSiteLinkResponse
{
    public required SiteLinkModel[] SiteLinks { get; set; }

    public class SiteLinkModel
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Url { get; set; }
    }
}
