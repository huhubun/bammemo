using Bammemo.Service.Abstractions.Dtos.Slips;
using Bammemo.Service.Abstractions.Paginations;

namespace Bammemo.Service.Abstractions;

public interface ICommonSlipService
{
    Task<ListSlipDto[]> ListAsync(
        ListSlipQueryRequestDto? query,
        CursorPagingRequest<string>? paging = null);
    Task<SlipDetailDto?> GetByIdAsync(string id);
    Task<SlipDetailDto?> GetByLinkNameAsync(string linkName);
}
