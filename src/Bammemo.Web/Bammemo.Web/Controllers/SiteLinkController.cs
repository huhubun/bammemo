using AutoMapper;
using Bammemo.Data.Entities;
using Bammemo.Service.Interfaces;
using Bammemo.Web.WebApiModels.SiteLinks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bammemo.Web.Controllers;

[Route("api/siteLinks")]
[ApiController]
public class SiteLinkController(
    IMapper mapper,
    ISiteLinkService siteLinkService) : BammemoControllerBase
{
    [HttpGet]
    [ProducesResponseType<ListSiteLinkResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> ListAsync()
    {
        var rules = await siteLinkService.ListFromCacheAsync();

        return Ok(new ListSiteLinkResponse
        {
            SiteLinks = mapper.Map<ListSiteLinkResponse.SiteLinkModel[]>(rules)
        });
    }

    [Authorize]
    [HttpPost]
    [ProducesResponseType<CreateSiteLinkResponse>(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateSiteLinkRequest request)
    {
        var entity = await siteLinkService.CreateAsync(mapper.Map<SiteLink>(request));
        return Created((string?)null, mapper.Map<CreateSiteLinkResponse>(entity));
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

        await siteLinkService.UpdateAsync(mapper.Map(request, entity));

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
