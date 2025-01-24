using AutoMapper;
using Bammemo.Data.Entities;
using Bammemo.Service.Abstractions.Paginations;
using Bammemo.Service.Abstractions.WebApiModels.Slips;
using Bammemo.Service.Server.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Bammemo.WebApi.Controllers;

[Route("slips")]
[ApiController]
public class SlipController(
    IMapper mapper,
    IIdService idService,
    ISlipService slipService) : BammemoControllerBase
{
    [HttpGet("")]
    public async Task<IActionResult> ListAsync([FromQuery] CursorPagingRequest<string>? paging)
    {
        var a = idService.DecodeAsync;
        var result = await slipService.ListAsync(await paging.DecodeAsync(idService.DecodeAsync) ?? null);

        return Ok(new ListSlipResponse
        {
            Data = mapper.Map<ListSlipResponse.SlipModel[]>(result)
        });
    }

    [HttpGet("{id}", Name = $"{nameof(SlipController)}_{nameof(GetByIdAsync)}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] string id)
    {
        return Ok(id);
    }

    [HttpPost("")]
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
}
