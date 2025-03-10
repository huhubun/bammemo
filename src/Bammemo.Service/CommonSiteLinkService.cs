using AutoMapper;
using Bammemo.Service.Abstractions;
using Bammemo.Service.Abstractions.Dtos.SiteLinks;
using Bammemo.Service.Interfaces;

namespace Bammemo.Service;

public class CommonSiteLinkService(
    IMapper mapper,
    ISiteLinkService siteLinkService
    ) : ICommonSiteLinkService
{
    public async Task<ListSiteLinkDto> ListAsync()
    {
        var result = await siteLinkService.ListFromCacheAsync();
        return new ListSiteLinkDto
        {
            SiteLinks = mapper.Map<ListSiteLinkDto.SiteLinkModel[]>(result)
        };
    }
}
