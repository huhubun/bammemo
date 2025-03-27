namespace Bammemo.Web.WebApiModels.SiteLinks
{
    public class CreateSiteLinkResponse
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Url { get; set; }
    }
}
