using Bammemo.Service.Abstractions.Dtos.Analytics;
using Bammemo.Service.Abstractions.WebApiModels.Slips;

namespace Bammemo.Service.Abstractions;

public interface ICommonAnalyticsService
{
    Task<GetSlipTagsDto> GetSlipTagsAsync();
    Task<GetSlipTimesDto> GetSlipTimesAsync(GetSlipTimesRequest request);
}
