using AutoMapper;
using Bammemo.Data.Entities;
using Bammemo.Service.Abstractions.Paginations;
using Bammemo.Service.Abstractions.WebApiModels.Slips;
using Bammemo.Service.Interfaces;
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
    public async Task<IActionResult> ListAsync(
        [FromQuery] ListSlipQueryRequest? query,
        [FromQuery] CursorPagingRequest<string>? paging)
    {
        var result = await slipService.ListAsync(
            query,
            await paging.DecodeAsync(idService.DecodeAsync) ?? null);

        return Ok(new ListSlipResponse
        {
            Data = mapper.Map<ListSlipResponse.SlipModel[]>(result)
        });
    }

    [HttpGet("{idOrLinkName}", Name = $"{nameof(SlipController)}_{nameof(GetByIdAsync)}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] string idOrLinkName, [FromQuery] GetSlipByIdRequest? request)
    {
        Slip? slip;

        if (request?.Type == SlipIdOrLinkNameType.LinkName)
        {
            slip = await slipService.GetByLinkNameAsync(idOrLinkName);
        }
        else
        {
            slip = await slipService.GetByIdNoTrackingAsync(await idService.DecodeAsync(idOrLinkName));
        }
        
        return slip != null ? Ok(mapper.Map<GetSlipByIdResponse>(slip)) : NotFound(idOrLinkName);
    }

    //[HttpPost("")]
    //public async Task<IActionResult> CreateAsync([FromBody] CreateSlipRequest request)
    //{
    //    var entity = mapper.Map<Slip>(request);

    //    var result = await slipService.CreateAsync(entity);

    //    return Created(
    //        nameof(GetByIdAsync),
    //        nameof(SlipController),
    //        await idService.EncodeAsync(result.Id),
    //        mapper.Map<CreateSlipResponse>(result));
    //}

    //[HttpPut("{id}")]
    //public async Task<IActionResult> UpdateAsync([FromRoute] string id, [FromBody] UpdateSlipRequest request)
    //{
    //    var rawId = await idService.DecodeAsync(id);
    //    var entity = await slipService.GetByIdAsync(rawId);

    //    if (entity == null)
    //    {
    //        return NotFound();
    //    }

    //    entity = mapper.Map(request, entity);

    //    var result = await slipService.UpdateAsync(entity);

    //    return Ok(mapper.Map<UpdateSlipResponse>(result));
    //}
}
