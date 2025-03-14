﻿using Bammemo.Data.Entities;
using Bammemo.Service.Abstractions.Paginations;
using Bammemo.Service.Abstractions.WebApiModels.Slips;

namespace Bammemo.Service.Interfaces;

public interface ISlipService
{
    Task<Slip[]> ListAsync(ListSlipQueryRequest? query, CursorPagingRequest<int>? paging);
    Task<Slip?> GetByIdAsync(int id);
    Task<Slip?> GetByIdNoTrackingAsync(int id);
    Task<Slip?> GetByLinkNameAsync(string linkName);
    Task<Slip> CreateAsync(Slip slip);
    Task<Slip> UpdateAsync(Slip slip);
    Task<int> DeleteAsync(int id);
    Task<long[]> GetCreatedTimeWithSlipAsync(long startTime, long endTime);
    Task<string[]> GetAllTagsAsync();
}
