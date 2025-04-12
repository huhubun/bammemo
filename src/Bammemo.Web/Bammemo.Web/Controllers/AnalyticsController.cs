using Bammemo.Service.Interfaces;
using Bammemo.Web.WebApiModels.Analytics;
using Microsoft.AspNetCore.Mvc;

namespace Bammemo.Web.Controllers;

[Route("api/analytics")]
[ApiController]
public class AnalyticsController(
    ISlipService slipService) : BammemoControllerBase
{
    [HttpGet("slips/times")]
    [ProducesResponseType<GetSlipTimesResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSlipTimesAsync([FromQuery] GetSlipTimesRequest request)
    {
        var times = await slipService.GetCreatedTimeWithSlipAsync(request.StartTime, request.EndTime);

        return Ok(new GetSlipTimesResponse
        {
            CreatedTimes = times
        });
    }

    [HttpGet("slips/tags")]
    [ProducesResponseType<GetSlipTagsResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetSlipTagsAsync([FromQuery] GetSlipTagsRequest request)
    {
        var tags = await slipService.GetTagsWithCountAsync(request?.Top);

        return Ok(new GetSlipTagsResponse
        {
            Tags = tags.Select(t => new GetSlipTagsResponse.TagItemAnalyticModel
            {
                Tag = t.Key,
                Count = t.Value
            }).ToArray()
        });
    }
}
