using AutoMapper;
using Bammemo.Service.Abstractions.WebApiModels.Settings;
using Bammemo.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace Bammemo.Web.Controllers;

[Route("api/settings")]
[ApiController]
public class SettingController(
    IMapper mapper,
    ISettingService settingService,
    ISecurityService securityService) : BammemoControllerBase
{
    [HttpGet("{key}")]
    [ProducesResponseType<GetSettingByKeyResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> GetByKeyAsync([FromRoute] string key)
    {
        if (!SettingKeys.VerifyKey(key))
        {
            return BadRequest();
        }

        if (SettingKeys.CheckProtectedSettingByKey(key) && !(HttpContext.User.Identity?.IsAuthenticated ?? false))
        {
            return Forbid();
        }

        var setting = await settingService.GetByKeyFromCacheAsync(key);

        if (setting != null)
        {
            return Ok(mapper.Map<GetSettingByKeyResponse>(setting));
        }

        return NotFound();
    }

    [Authorize]
    [HttpPut("{key}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> BatchUpdateByKeyAsync([FromRoute] string key, [FromBody] UpdateSettingByKeyRequest request)
    {
        if (!SettingKeys.VerifyKey(key))
        {
            return BadRequest();
        }

        await settingService.CreateOrUpdateAsync(key, request.Value);
        return NoContent();
    }

    [HttpGet("batch")]
    [ProducesResponseType<BatchGetSettingByKeyResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    public async Task<IActionResult> BatchGetByKeyAsync([FromQuery] BatchGetSettingByKeyRequest request)
    {
        if (!SettingKeys.VerifyKeys(request.Keys))
        {
            return BadRequest();
        }

        if (SettingKeys.CheckProtectedSettingByKeys(request.Keys) && !(HttpContext.User.Identity?.IsAuthenticated ?? false))
        {
            return Forbid();
        }

        var settings = await settingService.GetByKeysAsync(request.Keys);
        return Ok(new BatchGetSettingByKeyResponse
        {
            Settings = mapper.Map<BatchGetSettingByKeyResponse.SettingItemModel[]>(settings)
        });
    }

    [Authorize]
    [HttpPut("batch")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> BatchUpdateByKeyAsync([FromBody] BatchUpdateSettingByKeyRequest request)
    {
        if (!SettingKeys.VerifyKeys(request.Settings.Keys))
        {
            return BadRequest();
        }

        foreach (var setting in request.Settings)
        {
            await settingService.CreateOrUpdateAsync(setting.Key, setting.Value);
        }

        return NoContent();
    }

    [Authorize]
    [HttpGet("security/key-source")]
    [ProducesResponseType<GetKeySourceResponse>(StatusCodes.Status200OK)]
    public IActionResult GetKeySource()
    {
        return Ok(new GetKeySourceResponse
        {
            KeySource = securityService.GetKeySource()
        });
    }
}
