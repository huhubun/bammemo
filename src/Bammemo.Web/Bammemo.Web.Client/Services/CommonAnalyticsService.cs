using Bammemo.Service.Abstractions.Dtos.Analytics;
using Bammemo.Service.Abstractions.WebApiModels.Slips;

namespace Bammemo.Web.Client.Services;

public class CommonAnalyticsService(
    WebApiClient client
    ) : ICommonAnalyticsService
{
    public async Task<GetSlipTagsDto> GetSlipTagsAsync()
        => new GetSlipTagsDto
        {
            Tags = (await client.Analytics.GetSlipTagsAsync()).Tags
        };

    public async Task<GetSlipTimesDto> GetSlipTimesAsync(GetSlipTimesRequest request)
        => new GetSlipTimesDto
        {
            CreatedTimes = (await client.Analytics.GetSlipTimesAsync(request)).CreatedTimes
        };

}
