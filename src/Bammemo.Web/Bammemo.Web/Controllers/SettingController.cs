using AutoMapper;
using Bammemo.Service.Interfaces;
using Bammemo.Web.WebApiModels.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bammemo.Web.Controllers;

[Authorize]
[Route("api/settings")]
[ApiController]
public class SettingController(
    IMapper mapper,
    ISettingService settingService,
    ISecurityService securityService,
    IStorageService storageService) : BammemoControllerBase
{
    [AllowAnonymous]
    [HttpGet("{key}")]
    [ProducesResponseType<GetSettingByKeyResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByKeyAsync([FromRoute] string key)
    {
        if (!SettingKeys.VerifyKey(key))
        {
            return BadRequest();
        }

        if (SettingKeys.CheckProtectedSettingByKey(key) && !(HttpContext.User.Identity?.IsAuthenticated ?? false))
        {
            return Unauthorized();
        }

        var setting = await settingService.GetByKeyFromCacheAsync(key);

        if (setting != null)
        {
            return Ok(mapper.Map<GetSettingByKeyResponse>(setting));
        }

        return NotFound();
    }

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

    [AllowAnonymous]
    [HttpGet("batch")]
    [ProducesResponseType<BatchGetSettingByKeyResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> BatchGetByKeyAsync([FromQuery] BatchGetSettingByKeyRequest request)
    {
        if (!SettingKeys.VerifyKeys(request.Keys))
        {
            return BadRequest();
        }

        if (SettingKeys.CheckProtectedSettingByKeys(request.Keys) && !(HttpContext.User.Identity?.IsAuthenticated ?? false))
        {
            return Unauthorized();
        }

        var settings = await settingService.GetByKeysAsync(request.Keys);
        return Ok(new BatchGetSettingByKeyResponse
        {
            Settings = mapper.Map<BatchGetSettingByKeyResponse.SettingItemModel[]>(settings)
        });
    }

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

    [HttpGet("security/key-source")]
    [ProducesResponseType<GetKeySourceResponse>(StatusCodes.Status200OK)]
    public IActionResult GetKeySource()
    {
        return Ok(new GetKeySourceResponse
        {
            KeySource = securityService.GetKeySource()
        });
    }

    [HttpGet("storage/types")]
    [ProducesResponseType<GetStorageTypeInfosResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetStorageTypesAsync()
    {
        return Ok(new GetStorageTypeInfosResponse
        {
            StorageTypeInfos = mapper.Map<GetStorageTypeInfosResponse.StorageTypeInfoModel[]>(await storageService.GetStorageTypesAsync().ToListAsync())
        });
    }
}
