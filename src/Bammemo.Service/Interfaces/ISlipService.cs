using Bammemo.Data.Entities;
using Bammemo.Service.Abstractions.Dtos.Slips;
using Bammemo.Service.Abstractions.Paginations;

namespace Bammemo.Service.Interfaces;

public interface ISlipService
{
    Task<Slip[]> ListAsync(ListSlipQueryRequestDto? query, CursorPagingRequest<int>? paging);
    Task<Slip?> GetByIdAsync(int id);
    Task<Slip?> GetByIdNoTrackingAsync(int id);
    Task<Slip?> GetByLinkNameAsync(string linkName);
    Task<bool> CheckLinkNameExistsAsync(int currentSlipId, string linkName);
    Task<Slip> CreateAsync(Slip slip);
    Task<Slip> UpdateAsync(Slip slip);
    Task<int> DeleteAsync(int id);
    Task<long[]> GetCreatedTimeWithSlipAsync(long startTime, long endTime);
    Task<string[]> GetAllTagsAsync();
}
