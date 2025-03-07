using AutoMapper;
using Bammemo.Service.Abstractions.WebApiModels.Settings;
using Bammemo.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bammemo.Web.Controllers;

[Route("api/settings")]
[ApiController]
public class SettingController(
    IMapper mapper,
    ISettingService settingService) : BammemoControllerBase
{
    [HttpGet("{key}")]
    public async Task<IActionResult> GetByKeyAsync([FromRoute] string key)
    {
        var setting = await settingService.GetByKeyFromCacheAsync(key);
        if (setting != null)
        {
            return Ok(mapper.Map<GetSettingByKeyResponse>(setting));
        }

        return NotFound();
    }

    [HttpGet("batch")]
    public async Task<IActionResult> BatchGetByKeyAsync([FromQuery] BatchGetSettingByKeyRequest request)
    {
        var settings = await settingService.GetByKeysAsync(request.Keys);
        return Ok(new BatchGetSettingByKeyResponse
        {
            Settings = mapper.Map<List<BatchGetSettingByKeyResponse.SettingItemModel>>(settings)
        });
    }

    [Authorize]
    [HttpPut("batch")]
    public async Task<IActionResult> BatchUpdateByKeyAsync([FromBody] BatchUpdateSettingByKeyRequest request)
    {
        foreach (var setting in request.Settings)
        {
            await settingService.CreateOrUpdateAsync(setting.Key, setting.Value);
        }
        return NoContent();
    }
}
