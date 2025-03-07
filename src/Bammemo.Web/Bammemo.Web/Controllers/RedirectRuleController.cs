using AutoMapper;
using Bammemo.Data.Entities;
using Bammemo.Service.Abstractions.WebApiModels.RedirectRules;
using Bammemo.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bammemo.Web.Controllers;

[Authorize]
[Route("api/redirectRules")]
[ApiController]
public class RedirectRuleController(
    IMapper mapper,
    IRedirectRuleService redirectRuleService) : BammemoControllerBase
{
    [HttpGet]
    public async Task<IActionResult> ListAsync()
    {
        var rules = await redirectRuleService.ListAsync();

        return Ok(new ListRedirectRuleResponse
        {
            RedirectRules = mapper.Map<ListRedirectRuleResponse.RedirectRuleModel[]>(rules)
        });
    }

    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] CreateRedirectRuleRequest request)
    {
        var entity = await redirectRuleService.CreateAsync(mapper.Map<RedirectRule>(request));
        return Created((string?)null, mapper.Map<CreateRedirectRuleResponse>(entity));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] int id, [FromBody] UpdateRedirectRuleRequest request)
    {
        var entity = await redirectRuleService.GetByIdAsync(id);

        if (entity == null)
        {
            return NotFound();
        }

        await redirectRuleService.UpdateAsync(mapper.Map(request, entity));

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] int id)
    {
        var rows = await redirectRuleService.DeleteAsync(id);
        return rows == 0 ? NotFound() : NoContent();
    }

}
