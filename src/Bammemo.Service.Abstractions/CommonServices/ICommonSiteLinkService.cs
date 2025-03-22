using Bammemo.Service.Abstractions.Dtos.SiteLinks;

namespace Bammemo.Service.Abstractions.CommonServices;

public interface ICommonSiteLinkService
{
    Task<ListSiteLinkDto> ListAsync();
}
