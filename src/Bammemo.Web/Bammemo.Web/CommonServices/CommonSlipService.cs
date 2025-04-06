using Bammemo.Data.Entities;
using Bammemo.Service.Abstractions.CommonServices;
using Bammemo.Service.Abstractions.Dtos.Slips;
using Bammemo.Service.Abstractions.Paginations;
using Bammemo.Service.Interfaces;

namespace Bammemo.Web.CommonServices;

public class CommonSlipService(
    IIdService idService,
    ISlipService slipService) : ICommonSlipService
{
    public async Task<ListSlipDto[]> ListAsync(
        ListSlipQueryRequestDto? query,
        CursorPagingRequest<string>? paging = null)
    {
        var slips = await slipService.ListAsync(query, await paging.DecodeAsync(idService.DecodeAsync) ?? null);
        var attachmentsGroup = await slipService.LoadAttachmentsAsync(slips.Select(r => r.Id));

        var result = new List<ListSlipDto>();
        foreach (var item in slips)
        {
            var dto = item.MapTo<ListSlipDto>();
            dto.Id =  await idService.EncodeAsync(item.Id);

            if (attachmentsGroup.TryGetValue(item.Id, out var attachments))
            {
                dto.Attachments = attachments;
            }

            result.Add(dto);
        }

        return result.ToArray();
    }

    public async Task<SlipDetailDto?> GetByIdAsync(string id)
    {
        var slip = await slipService.GetByIdNoTrackingAsync(await idService.DecodeAsync(id));
        return slip != null ? await ConvertToSlipDetailDto(slip) : null;
    }

    public async Task<SlipDetailDto?> GetByLinkNameAsync(string linkName)
    {
        var slip = await slipService.GetByLinkNameAsync(linkName);
        return slip != null ? await ConvertToSlipDetailDto(slip) : null;
    }

    private async Task<SlipDetailDto> ConvertToSlipDetailDto(Slip entity)
    {
        var dto = entity.MapTo<SlipDetailDto>();
        dto.Id = await idService.EncodeAsync(entity.Id);

        return dto;
    }
}
