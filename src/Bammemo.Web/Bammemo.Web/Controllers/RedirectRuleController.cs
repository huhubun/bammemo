using Bammemo.Data.Entities;
using Bammemo.Service.Interfaces;
using Bammemo.Web.WebApiModels.RedirectRules;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bammemo.Web.Controllers;

[Authorize]
[Route("api/redirect-rules")]
[ApiController]
public class RedirectRuleController(
    IRedirectRuleService redirectRuleService) : BammemoControllerBase
{
    [HttpGet]
    [ProducesResponseType<ListRedirectRuleResponse>(StatusCodes.Status200OK)]
    public async Task<IActionResult> ListAsync()
    {
        var rules = await redirectRuleService.ListAsync();

        return Ok(new ListRedirectRuleResponse
        {
            RedirectRules = rules.MapToArray<ListRedirectRuleResponse.RedirectRuleModel>()
        });
    }

    [HttpPost]
    [ProducesResponseType<CreateRedirectRuleResponse>(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateRedirectRuleRequest request)
    {
        var entity = await redirectRuleService.CreateAsync(request.MapTo<RedirectRule>());
        return Created((string?)null, entity.MapTo<CreateRedirectRuleResponse>());
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] UpdateRedirectRuleRequest request)
    {
        var entity = await redirectRuleService.GetByIdAsync(id);

        if (entity == null)
        {
            return NotFound();
        }

        await redirectRuleService.UpdateAsync(request.MapTo(entity));

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        var rows = await redirectRuleService.DeleteAsync(id);
        return rows == 0 ? NotFound() : NoContent();
    }
}
