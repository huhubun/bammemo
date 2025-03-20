using AutoMapper;
using Bammemo.Data.Entities;
using Bammemo.Service.Abstractions.Dtos.Slips;
using Bammemo.Service.Abstractions.Paginations;
using Bammemo.Service.Interfaces;
using Bammemo.Web.WebApiModels.Slips;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bammemo.Web.Controllers;

[Route("api/slips")]
[ApiController]
public class SlipController(
    IMapper mapper,
    IIdService idService,
    ISlipService slipService) : BammemoControllerBase
{
    [HttpGet("")]
    [ProducesResponseType<ListSlipResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public async Task<IActionResult> ListAsync(
        [FromQuery] ListSlipQueryRequest? query,
        [FromQuery] CursorPagingRequest<string>? paging)
    {
        try
        {
            var result = await slipService.ListAsync(
                mapper.Map<ListSlipQueryRequestDto>(query),
                await paging.DecodeAsync(idService.DecodeAsync) ?? null);

            return Ok(new ListSlipResponse
            {
                Data = mapper.Map<ListSlipResponse.SlipModel[]>(result)
            });
        }
        catch (UnauthorizedAccessException)
        {
            return Unauthorized();
        }
    }

    [HttpGet("{id}", Name = $"{nameof(SlipController)}_{nameof(GetByIdAsync)}")]
    [ProducesResponseType<GetSlipByIdResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] string id)
    {
        var slip = await slipService.GetByIdNoTrackingAsync(await idService.DecodeAsync(id));
        return slip != null ? Ok(mapper.Map<GetSlipByIdResponse>(slip)) : NotFound(id);
    }

    [HttpGet("link/{linkName}")]
    [ProducesResponseType<GetSlipByLinkNameResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByLinkNameAsync([FromRoute] string linkName)
    {
        var slip = await slipService.GetByLinkNameAsync(linkName);
        return slip != null ? Ok(mapper.Map<GetSlipByLinkNameResponse>(slip)) : NotFound(linkName);
    }

    [Authorize]
    [HttpPost("")]
    [ProducesResponseType<CreateSlipResponse>(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateSlipRequest request)
    {
        var entity = mapper.Map<Slip>(request);

        var result = await slipService.CreateAsync(entity);

        return Created(
            nameof(GetByIdAsync),
            nameof(SlipController),
            await idService.EncodeAsync(result.Id),
            mapper.Map<CreateSlipResponse>(result));
    }

    [Authorize]
    [HttpPut("{id}")]
    [ProducesResponseType<UpdateSlipResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateAsync([FromRoute] string id, [FromBody] UpdateSlipRequest request)
    {
        var rawId = await idService.DecodeAsync(id);
        var entity = await slipService.GetByIdAsync(rawId);

        if (entity == null)
        {
            return NotFound();
        }

        entity = mapper.Map(request, entity);

        var result = await slipService.UpdateAsync(entity);

        return Ok(mapper.Map<UpdateSlipResponse>(result));
    }

    [Authorize]
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute] string id)
    {
        var rawId = await idService.DecodeAsync(id);
        var rows = await slipService.DeleteAsync(rawId);

        return rows == 0 ? NotFound() : NoContent();
    }
}
