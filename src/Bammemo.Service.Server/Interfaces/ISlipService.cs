using Bammemo.Data.Entities;
using Bammemo.Service.Abstractions.Paginations;
using Bammemo.Service.Abstractions.WebApiModels.Slips;

namespace Bammemo.Service.Server.Interfaces;

public interface ISlipService
{
    Task<Slip[]> ListAsync(ListSlipQueryRequest? query, CursorPagingRequest<int>? paging);
    Task<Slip?> GetByIdAsync(int id);
    Task<Slip> CreateAsync(Slip slip);
    Task<Slip> UpdateAsync(Slip slip);
    Task<long[]> GetCreatedTimeWithSlipAsync(long startTime, long endTime);
}
