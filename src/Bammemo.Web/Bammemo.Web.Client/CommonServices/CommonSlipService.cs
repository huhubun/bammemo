﻿using AutoMapper;
using Bammemo.Service.Abstractions.CommonServices;
using Bammemo.Service.Abstractions.Dtos.Slips;
using Bammemo.Service.Abstractions.Paginations;

namespace Bammemo.Web.Client.CommonServices;

public class CommonSlipService(
    IMapper mapper,
    WebApis.Client.WebApiClient client) : ICommonSlipService
{
    public async Task<ListSlipDto[]> ListAsync(
        ListSlipQueryRequestDto? query, 
        CursorPagingRequest<string>? paging = null)
    {
        var result = await client.Api.Slips.GetAsync(c => c.QueryParameters = new WebApis.Client.Api.Slips.SlipsRequestBuilder.SlipsRequestBuilderGetQueryParameters
        {
            StartTime = query.StartTime,
            EndTime = query.EndTime,
            Tags = query.Tags,
            Status = query.Status?.Cast<int?>().ToArray() ?? null,
            Cursor = paging?.Cursor,
            Take = paging?.Take
        });
        return mapper.Map<ListSlipDto[]>(result?.Data);
    }

    public async Task<SlipDetailDto?> GetByIdAsync(string id)
    {
        var result = await client.Api.Slips[id].GetAsync();
        return mapper.Map<SlipDetailDto>(result);
    }

    public async Task<SlipDetailDto?> GetByLinkNameAsync(string linkName)
    {
        var result = await client.Api.Slips.Link[linkName].GetAsync();
        return mapper.Map<SlipDetailDto>(result);
    }
}
