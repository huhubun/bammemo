using Bammemo.Service.Abstractions.Paginations;
using Bammemo.Service.Abstractions.WebApiModels.Slips;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;

namespace Bammemo.Web.Client.Services;

public class WebApiClient(HttpClient httpClient)
{
    public SlipClient Slips { get; } = new SlipClient(httpClient);

    public class SlipClient(HttpClient httpClient)
    {
        public async Task<ListSlipResponse?> ListAsync(CursorPagingRequest<string>? paging)
        {
            var query = QueryHelpers.AddQueryString(String.Empty, paging.ToDictionary());
            var result = await httpClient.GetFromJsonAsync<ListSlipResponse>("slips" + query);

            return result;
        }
    }
}
