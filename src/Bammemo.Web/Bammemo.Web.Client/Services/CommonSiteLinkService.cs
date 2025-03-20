using AutoMapper;
using Bammemo.Service.Abstractions.Dtos.SiteLinks;

namespace Bammemo.Web.Client.Services;

public class CommonSiteLinkService(
    IMapper mapper,
    Bammemo.Web.Client.WebApis.Client.WebApiClient client
    ) : ICommonSiteLinkService
{
    public async Task<ListSiteLinkDto> ListAsync()
    {
        var response = await client.Api.SiteLinks.GetAsync();
        return mapper.Map<ListSiteLinkDto>(response);
    }
}
