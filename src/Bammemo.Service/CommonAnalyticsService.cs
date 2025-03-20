using Bammemo.Service.Abstractions;
using Bammemo.Service.Abstractions.Dtos.Analytics;
using Bammemo.Service.Interfaces;

namespace Bammemo.Service;

public class CommonAnalyticsService(
    ISlipService slipService) : ICommonAnalyticsService
{
    public async Task<GetSlipTagsDto> GetSlipTagsAsync()
        => new GetSlipTagsDto
        {
            Tags = await slipService.GetAllTagsAsync()
        };

    public async Task<GetSlipTimesDto> GetSlipTimesAsync(long startTime, long endTime)
        => new GetSlipTimesDto
        {
            CreatedTimes = await slipService.GetCreatedTimeWithSlipAsync(startTime, endTime)
        };
}
