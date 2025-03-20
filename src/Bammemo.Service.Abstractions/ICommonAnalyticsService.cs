using Bammemo.Service.Abstractions.Dtos.Analytics;

namespace Bammemo.Service.Abstractions;

public interface ICommonAnalyticsService
{
    Task<GetSlipTagsDto> GetSlipTagsAsync();
    Task<GetSlipTimesDto> GetSlipTimesAsync(long startTime, long endTime);
}
