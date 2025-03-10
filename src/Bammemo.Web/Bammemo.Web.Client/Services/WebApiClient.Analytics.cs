using Bammemo.Service.Abstractions.Dtos.Analytics;
using Bammemo.Service.Abstractions.WebApiModels.Slips;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;

namespace Bammemo.Web.Client.Services;

public partial class WebApiClient
{
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
            var response = await httpClient.GetFromJsonAsync<GetSlipTimesResponse>("analytics/slips/times" + query, SourceGenerationContext.Default.GetSlipTimesResponse);

            ArgumentNullException.ThrowIfNull(response);

            return response;
        }

        public async Task<GetSlipTagsResponse> GetSlipTagsAsync()
        {
            var response = await httpClient.GetFromJsonAsync<GetSlipTagsResponse>("analytics/slips/tags", SourceGenerationContext.Default.GetSlipTagsResponse);
            return response;
        }
    }
}
