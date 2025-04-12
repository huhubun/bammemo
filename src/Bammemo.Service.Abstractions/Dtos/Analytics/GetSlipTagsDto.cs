namespace Bammemo.Service.Abstractions.Dtos.Analytics;

public class GetSlipTagsDto
{
    public required TagItemAnalyticModel[] Tags { get; set; }

    public record TagItemAnalyticModel
    {
        public required string Tag { get; set; }
        public int Count { get; set; }
    }
}
