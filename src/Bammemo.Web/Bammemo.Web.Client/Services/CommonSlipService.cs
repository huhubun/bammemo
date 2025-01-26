using AutoMapper;
using Bammemo.Service.Abstractions.Dtos;
using Bammemo.Service.Abstractions.Paginations;

namespace Bammemo.Web.Client.Services;

public class CommonSlipService(
    IMapper mapper,
    WebApiClient client) : ICommonSlipService
{
    public async Task<ListSlipDto[]> ListAsync(CursorPagingRequest<string>? paging)
    {
        var result = await client.Slips.ListAsync(paging);
        return mapper.Map<ListSlipDto[]>(result?.Data);
    }
}
