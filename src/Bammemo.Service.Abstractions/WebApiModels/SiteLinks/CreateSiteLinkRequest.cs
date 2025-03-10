namespace Bammemo.Service.Abstractions.WebApiModels.SiteLinks
{
    public class CreateSiteLinkRequest
    {
        public required string Name { get; set; }
        public required string Url { get; set; }
    }
}
