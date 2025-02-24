using AutoMapper;
using Bammemo.Data.Entities;
using Bammemo.Service.Abstractions;
using Bammemo.Service.Abstractions.Dtos;
using Bammemo.Service.Abstractions.Enums;
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
        return mapper.Map<ListSlipDto[]>(result);
    }

    public async Task<SlipDetailDto?> GetByIdAsync(string idOrLinkName, GetSlipByIdRequest? request = null)
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

        return slip != null ? mapper.Map<SlipDetailDto>(slip) : null;
    }

}
