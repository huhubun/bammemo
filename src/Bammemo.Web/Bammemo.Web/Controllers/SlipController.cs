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
                query.MapTo<ListSlipQueryRequestDto>(),
                await paging.DecodeAsync(idService.DecodeAsync) ?? null);

            var slipModels = new List<ListSlipResponse.SlipModel>();
            foreach (var item in result)
            {
                var model = item.MapTo<ListSlipResponse.SlipModel>();
                model.Id = await idService.EncodeAsync(item.Id);

                slipModels.Add(model);
            }

            return Ok(new ListSlipResponse
            {
                Data = [.. slipModels]
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
        return slip != null ? Ok(slip.MapTo<GetSlipByIdResponse>()) : NotFound(id);
    }

    [HttpGet("link/{linkName}")]
    [ProducesResponseType<GetSlipByLinkNameResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetByLinkNameAsync([FromRoute] string linkName)
    {
        var slip = await slipService.GetByLinkNameAsync(linkName);
        return slip != null ? Ok(slip.MapTo<GetSlipByLinkNameResponse>()) : NotFound(linkName);
    }

    [Authorize]
    [HttpPost("")]
    [ProducesResponseType<CreateSlipResponse>(StatusCodes.Status201Created)]
    public async Task<IActionResult> CreateAsync([FromBody] CreateSlipRequest request)
    {
        var entity = request.MapTo<Slip>();

        var result = await slipService.CreateAsync(entity);
        var encodedId = await idService.EncodeAsync(result.Id);

        var model = result.MapTo<CreateSlipResponse>();
        model.Id = encodedId;

        return Created(
            nameof(GetByIdAsync),
            nameof(SlipController),
            encodedId,
            model);
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

        entity = request.MapTo(entity);

        var result = await slipService.UpdateAsync(entity);

        return Ok(result.MapTo<UpdateSlipResponse>());
    }

    [Authorize]
    [HttpPut("{id}/property")]
    [ProducesResponseType<UpdateSlipResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<IActionResult> UpdatePropertyAsync([FromRoute] string id, [FromBody] UpdateSlipPropertyRequest request)
    {
        var rawId = await idService.DecodeAsync(id);
        var entity = await slipService.GetByIdAsync(rawId);

        if (entity == null)
        {
            return NotFound();
        }

        if (request.FriendlyLinkName != null && await slipService.CheckLinkNameExistsAsync(rawId, request.FriendlyLinkName))
        {
            return Conflict();
        }

        entity = request.MapTo(entity);

        var result = await slipService.UpdateAsync(entity);

        return Ok(result.MapTo<UpdateSlipResponse>());
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
