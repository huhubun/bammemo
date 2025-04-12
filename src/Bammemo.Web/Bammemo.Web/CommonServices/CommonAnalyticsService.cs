using Bammemo.Service.Abstractions.CommonServices;
using Bammemo.Service.Abstractions.Dtos.Analytics;
using Bammemo.Service.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace Bammemo.Web.CommonServices;

public class CommonAnalyticsService(
    ISlipService slipService) : ICommonAnalyticsService
{
    public async Task<GetSlipTagsDto> GetSlipTagsAsync(int? top)
        => new GetSlipTagsDto
        {
            Tags = (await slipService.GetTagsWithCountAsync(top)).Select(t => new GetSlipTagsDto.TagItemAnalyticModel
            {
                Tag = t.Key,
                Count = t.Value
            }).ToArray()
        };


    public async Task<GetSlipTimesDto> GetSlipTimesAsync(long startTime, long endTime)
        => new GetSlipTimesDto
        {
            CreatedTimes = await slipService.GetCreatedTimeWithSlipAsync(startTime, endTime)
        };
}
