using AutoMapper;
using Bammemo.Service.Abstractions;
using Bammemo.Service.Abstractions.Dtos;
using Bammemo.Service.Abstractions.Paginations;
using Bammemo.Service.Server.Interfaces;

namespace Bammemo.Service.Server;

public class CommonSlipService(
    IMapper mapper,
    IIdService idService,
    ISlipService slipService) : ICommonSlipService
{
    public async Task<ListSlipDto[]> ListAsync(CursorPagingRequest<string>? paging)
    {
        var result = await slipService.ListAsync(await paging.DecodeAsync(idService.DecodeAsync) ?? null);
        return mapper.Map<Dtos.ListSlipDto[]>(result);
    }
}
