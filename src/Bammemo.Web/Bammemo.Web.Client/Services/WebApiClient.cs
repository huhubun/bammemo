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
        public async Task<ListSlipResponse?> ListAsync(CursorPagingRequest<string>? request)
        {
            var query = QueryHelpers.AddQueryString(String.Empty, request.ToDictionary());
            var response = await httpClient.GetFromJsonAsync<ListSlipResponse>("slips" + query);

            return response;
        }

        public async Task<CreateSlipResponse?> CreateAsync(CreateSlipRequest request)
        {
            var responseMessage = await httpClient.PostAsJsonAsync("slips", request);
            var response = await responseMessage.Content.ReadFromJsonAsync<CreateSlipResponse>();

            return response;
        }
    }
}
