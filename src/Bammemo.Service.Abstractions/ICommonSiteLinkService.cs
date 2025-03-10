using Bammemo.Service.Abstractions.Dtos.SiteLinks;

namespace Bammemo.Service.Abstractions;

public interface ICommonSiteLinkService
{
    Task<ListSiteLinkDto> ListAsync();
}
