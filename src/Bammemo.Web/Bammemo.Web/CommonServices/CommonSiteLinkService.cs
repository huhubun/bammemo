using Bammemo.Service.Abstractions.CommonServices;
using Bammemo.Service.Abstractions.Dtos.SiteLinks;
using Bammemo.Service.Interfaces;

namespace Bammemo.Web.CommonServices;

public class CommonSiteLinkService(
    ISiteLinkService siteLinkService
    ) : ICommonSiteLinkService
{
    public async Task<ListSiteLinkDto> ListAsync()
    {
        var result = await siteLinkService.ListFromCacheAsync();
        return new ListSiteLinkDto
        {
            SiteLinks = result.MapToArray<ListSiteLinkDto.SiteLinkModel>()
        };
    }
}
