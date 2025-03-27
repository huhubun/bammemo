﻿using Bammemo.Service.Abstractions.CommonServices;
using Bammemo.Service.Abstractions.Dtos.Analytics;

namespace Bammemo.Web.Client.CommonServices;

public class CommonAnalyticsService(
    WebApis.Client.WebApiClient client) : ICommonAnalyticsService
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
