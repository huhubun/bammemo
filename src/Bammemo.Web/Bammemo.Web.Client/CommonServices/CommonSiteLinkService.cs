using Bammemo.Service.Abstractions.CommonServices;
using Bammemo.Service.Abstractions.Dtos.SiteLinks;

namespace Bammemo.Web.Client.CommonServices;

public class CommonSiteLinkService(
    WebApis.Client.WebApiClient client) : ICommonSiteLinkService
{
    public async Task<ListSiteLinkDto> ListAsync()
    {
        var response = await client.Api.SiteLinks.GetAsync();
        return response.MapTo<ListSiteLinkDto>();
    }
}
