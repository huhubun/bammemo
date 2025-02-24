using Bammemo.Service.Abstractions.Paginations;
using Bammemo.Service.Abstractions.WebApiModels.Slips;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;

namespace Bammemo.Web.Client.Services;

public class WebApiClient(HttpClient httpClient)
{
    public SlipClient Slips { get; } = new SlipClient(httpClient);
    public AnalyticsClient Analytics { get; } = new AnalyticsClient(httpClient);

    public class SlipClient(HttpClient httpClient)
    {
        public async Task<ListSlipResponse?> ListAsync(
            ListSlipQueryRequest? query,
            CursorPagingRequest<string>? paging)
        {
            var requestParameters = paging?.ToQueryStringParameters() ?? [];
            if (query != null)
            {
                if (query.StartTime.HasValue && query.EndTime.HasValue)
                {
                    requestParameters.Add(new KeyValuePair<string, string?>(nameof(query.StartTime), query.StartTime.ToString()));
                    requestParameters.Add(new KeyValuePair<string, string?>(nameof(query.EndTime), query.EndTime.ToString()));
                }

                if (query.Tags != null)
                {
                    foreach (var tag in query.Tags)
                    {
                        requestParameters.Add(new KeyValuePair<string, string?>(nameof(query.Tags), tag));
                    }
                }
            }

            var queryString = QueryHelpers.AddQueryString(String.Empty, requestParameters);
            var response = await httpClient.GetFromJsonAsync<ListSlipResponse>("slips" + queryString);

            return response;
        }

        public async Task<GetSlipByIdResponse?> GetByIdAsync(string idOrLinkName, GetSlipByIdRequest? request = null)
        {
            var queryString = request?.Type != null ? QueryHelpers.AddQueryString(String.Empty, nameof(request.Type), request.Type.Value.ToString()) : null;
            return await httpClient.GetFromJsonAsync<GetSlipByIdResponse?>($"slips/{idOrLinkName}" + queryString);
        }

        public async Task<CreateSlipResponse?> CreateAsync(CreateSlipRequest request)
        {
            var responseMessage = await httpClient.PostAsJsonAsync("slips", request);
            var response = await responseMessage.Content.ReadFromJsonAsync<CreateSlipResponse>();

            return response;
        }

        public async Task<UpdateSlipResponse?> UpdateAsync(string id, UpdateSlipRequest request)
        {
            var responseMessage = await httpClient.PutAsJsonAsync($"slips/{id}", request);
            var response = await responseMessage.Content.ReadFromJsonAsync<UpdateSlipResponse>();

            return response;
        }
    }

    public class AnalyticsClient(HttpClient httpClient)
    {
        public async Task<GetSlipTimesResponse> GetSlipTimesAsync(GetSlipTimesRequest request)
        {
            var query = QueryHelpers.AddQueryString(
                String.Empty,
                new Dictionary<string, string?>
                {
                    {nameof(request.StartTime), request.StartTime.ToString() },
                    {nameof(request.EndTime), request.EndTime.ToString() }
                });
            var response = await httpClient.GetFromJsonAsync<GetSlipTimesResponse>("analytics/slips/times" + query);

            ArgumentNullException.ThrowIfNull(response);

            return response;
        }

        public async Task<GetSlipTagsResponse> GetSlipTagsAsync()
        {
            var response = await httpClient.GetFromJsonAsync<GetSlipTagsResponse>("analytics/slips/tags");
            return response;
        }
    }
}
