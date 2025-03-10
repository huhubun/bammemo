namespace Bammemo.Service.Abstractions.WebApiModels.SiteLinks;

public class UpdateSiteLinkRequest
{
    public required string Name { get; set; }
    public required string Url { get; set; }
}
