using AutoMapper;
using Bammemo.Service.Abstractions;
using Bammemo.Service.Abstractions.Dtos;
using Bammemo.Service.Abstractions.Paginations;
using Bammemo.Service.Abstractions.WebApiModels.Slips;
using Bammemo.Service.Server.Interfaces;

namespace Bammemo.Service.Server;

public class CommonSlipService(
    IMapper mapper,
    IIdService idService,
    ISlipService slipService) : ICommonSlipService
{
    public async Task<ListSlipDto[]> ListAsync(
        ListSlipQueryRequest? query, 
        CursorPagingRequest<string>? paging = null)
    {
        var result = await slipService.ListAsync(query, await paging.DecodeAsync(idService.DecodeAsync) ?? null);
        return mapper.Map<Dtos.ListSlipDto[]>(result);
    }
}
