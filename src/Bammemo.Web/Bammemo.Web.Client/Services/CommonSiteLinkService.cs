using AutoMapper;
using Bammemo.Service.Abstractions.Dtos.SiteLinks;

namespace Bammemo.Web.Client.Services;

public class CommonSiteLinkService(
    IMapper mapper,
    WebApiClient client
    ) : ICommonSiteLinkService
{
    public async Task<ListSiteLinkDto> ListAsync()
    {
        var response = await client.SiteLinks.ListAsync();
        return mapper.Map<ListSiteLinkDto>(response);
    }
}
