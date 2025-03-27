using Bammemo.Data.Entities;
using Bammemo.Service.Interfaces;
using Bammemo.Web.WebApiModels.SiteLinks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bammemo.Web.Controllers;

[Route("api/siteLinks")]
[ApiController]
public class SiteLinkController(
    ISiteLinkService siteLinkService) : BammemoControllerBase
{
    [HttpGet]
    [ProducesResponseType<ListSiteLinkResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> ListAsync()
    {
        var rules = await siteLinkService.ListFromCacheAsync();

        return Ok(new ListSiteLinkResponse
        {
            SiteLinks = rules.MapToArray<ListSiteLinkResponse.SiteLinkModel>()
        });
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType<CreateSiteLinkResponse>(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateSiteLinkRequest request)
    {
        var entity = await siteLinkService.CreateAsync(request.MapTo<SiteLink>());
        return Created((string?)null, entity.MapTo<CreateSiteLinkResponse>());
    }

    [Authorize]
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] UpdateSiteLinkRequest request)
    {
        var entity = await siteLinkService.GetByIdAsync(id);

        if (entity == null)
        {
            return NotFound();
        }

        await siteLinkService.UpdateAsync(request.MapTo(entity));

        return NoContent();
    }

    [Authorize]
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        var rows = await siteLinkService.DeleteAsync(id);
        return rows == 0 ? NotFound() : NoContent();
    }
}
