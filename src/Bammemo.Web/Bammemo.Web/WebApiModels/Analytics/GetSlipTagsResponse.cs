namespace Bammemo.Web.WebApiModels.Analytics;

public class GetSlipTagsResponse
{
    public required TagItemAnalyticModel[] Tags { get; set; }

    public record TagItemAnalyticModel
    {
        public required string Tag { get; set; }
        public int Count { get; set; }
    }
}
