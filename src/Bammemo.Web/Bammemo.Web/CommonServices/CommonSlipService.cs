using AutoMapper;
using Bammemo.Service.Abstractions.CommonServices;
using Bammemo.Service.Abstractions.Dtos.Slips;
using Bammemo.Service.Abstractions.Paginations;
using Bammemo.Service.Interfaces;

namespace Bammemo.Web.CommonServices;

public class CommonSlipService(
    IMapper mapper,
    IIdService idService,
    ISlipService slipService) : ICommonSlipService
{
    public async Task<ListSlipDto[]> ListAsync(
        ListSlipQueryRequestDto? query,
        CursorPagingRequest<string>? paging = null)
    {
        var result = await slipService.ListAsync(query, await paging.DecodeAsync(idService.DecodeAsync) ?? null);
        return mapper.Map<ListSlipDto[]>(result);
    }

    public async Task<SlipDetailDto?> GetByIdAsync(string id)
    {
        var slip = await slipService.GetByIdNoTrackingAsync(await idService.DecodeAsync(id));
        return slip != null ? mapper.Map<SlipDetailDto>(slip) : null;
    }

    public async Task<SlipDetailDto?> GetByLinkNameAsync(string linkName)
    {
        var slip = await slipService.GetByLinkNameAsync(linkName);
        return slip != null ? mapper.Map<SlipDetailDto>(slip) : null;
    }
}
