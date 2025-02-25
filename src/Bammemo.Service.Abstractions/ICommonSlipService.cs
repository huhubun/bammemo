using Bammemo.Service.Abstractions.Paginations;
using Bammemo.Service.Abstractions.WebApiModels.Slips;

namespace Bammemo.Service.Abstractions;

public interface ICommonSlipService
{
    Task<ListSlipDto[]> ListAsync(
        ListSlipQueryRequest? query,
        CursorPagingRequest<string>? paging = null);

    Task<SlipDetailDto?> GetByIdOrLinkNameAsync(string idOrLinkName, GetSlipByIdRequest? request = null);
}
