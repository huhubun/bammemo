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
        public async Task<ListSlipResponse?> ListAsync(
            ListSlipQueryRequest? query,
            CursorPagingRequest<string>? paging)
        {
            var requestParameters = paging?.ToDictionary() ?? [];
            if (query != null)
            {
                if (query.StartTime.HasValue && query.EndTime.HasValue)
                {
                    requestParameters.Add(nameof(query.StartTime), query.StartTime.ToString());
                    requestParameters.Add(nameof(query.EndTime), query.EndTime.ToString());
                }
            }

            var queryString = QueryHelpers.AddQueryString(String.Empty, requestParameters);
            var response = await httpClient.GetFromJsonAsync<ListSlipResponse>("slips" + queryString);

            return response;
        }

        public async Task<CreateSlipResponse?> CreateAsync(CreateSlipRequest request)
        {
            var responseMessage = await httpClient.PostAsJsonAsync("slips", request);
            var response = await responseMessage.Content.ReadFromJsonAsync<CreateSlipResponse>();

            return response;
        }

        public async Task UpdateAsync(string id, UpdateSlipRequest request)
        {
            var responseMessage = await httpClient.PutAsJsonAsync($"slips/{id}", request);
            if (!responseMessage.IsSuccessStatusCode)
            {
                throw new InvalidOperationException(await responseMessage.Content.ReadAsStringAsync());
            }
        }

        public async Task<GetSlipTimesResponse> GetSlipTimesAsync(GetSlipTimesRequest request)
        {
            var query = QueryHelpers.AddQueryString(
                String.Empty,
                new Dictionary<string, string?>
                {
                    {nameof(request.StartTime), request.StartTime.ToString() },
                    {nameof(request.EndTime), request.EndTime.ToString() }
                });
            var response = await httpClient.GetFromJsonAsync<GetSlipTimesResponse>("slips/times" + query);

            ArgumentNullException.ThrowIfNull(response);

            return response;
        }
    }
}
