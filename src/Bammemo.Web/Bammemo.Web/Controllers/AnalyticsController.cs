﻿using Bammemo.Service.Abstractions.Dtos.Analytics;
using Bammemo.Service.Abstractions.WebApiModels.Slips;
using Bammemo.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bammemo.Web.Controllers;

[Route("api/analytics")]
public class AnalyticsController(
    ISlipService slipService) : BammemoControllerBase
{
    [HttpGet("slips/times")]
    public async Task<IActionResult> GetSlipTimesAsync([FromQuery] GetSlipTimesRequest request)
    {
        var times = await slipService.GetCreatedTimeWithSlipAsync(request.StartTime, request.EndTime);
        
        return Ok(new GetSlipTimesResponse
        {
            CreatedTimes = times
        });
    }

    [HttpGet("slips/tags")]
    public async Task<IActionResult> GetSlipTagsAsync()
    {
        var tags = await slipService.GetAllTagsAsync();

        return Ok(new GetSlipTagsResponse
        {
            Tags = tags
        });
    }
}
