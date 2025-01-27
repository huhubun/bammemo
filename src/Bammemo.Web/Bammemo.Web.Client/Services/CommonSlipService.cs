using AutoMapper;
using Bammemo.Service.Abstractions.Paginations;
using Bammemo.Service.Abstractions.WebApiModels.Slips;

namespace Bammemo.Web.Client.Services;

public class CommonSlipService(
    IMapper mapper,
    WebApiClient client) : ICommonSlipService
{
    public async Task<ListSlipDto[]> ListAsync(
        ListSlipQueryRequest? query, 
        CursorPagingRequest<string>? paging = null)
    {
        var result = await client.Slips.ListAsync(query, paging);
        return mapper.Map<ListSlipDto[]>(result?.Data);
    }
}
