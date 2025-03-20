using AutoMapper;
using Bammemo.Service.Abstractions.Dtos.Analytics;

namespace Bammemo.Web.Client.Services;

public class CommonAnalyticsService(
    IMapper mapper,
    Bammemo.Web.Client.WebApis.Client.WebApiClient client) : ICommonAnalyticsService
{
    public async Task<GetSlipTagsDto> GetSlipTagsAsync()
        => new GetSlipTagsDto
        {
            Tags = (await client.Api.Analytics.Slips.Tags.GetAsync()).Tags.ToArray()
        };

    public async Task<GetSlipTimesDto> GetSlipTimesAsync(long startTime, long endTime)
        => new GetSlipTimesDto
        {
            CreatedTimes = (await client.Api.Analytics.Slips.Times.GetAsync(c => {
                c.QueryParameters.StartTime = startTime;
                c.QueryParameters.EndTime = endTime;
            })).CreatedTimes.Cast<long>().ToArray()
        };

}
