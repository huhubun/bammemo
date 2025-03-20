using Bammemo.Service.Abstractions.WebApiModels.Settings;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;

namespace Bammemo.Web.Client.Services;

public partial class WebApiClient
{
    public class SettingClient(HttpClient httpClient)
    {
        public async Task<GetSettingByKeyResponse?> GetByKeyAsync(string key)
        {
            var response = await httpClient.GetFromJsonAsync($"settings/{key}", SourceGenerationContext.Default.GetSettingByKeyResponse);
            return response;
        }

        public async Task UpdateByKeyAsync(string key, UpdateSettingByKeyRequest request)
        {
            var response = await httpClient.PutAsync($"settings/{key}", JsonContent.Create(request, SourceGenerationContext.Default.BatchUpdateSettingByKeyRequest));
            if (!response.IsSuccessStatusCode)
            {
                // TODO
            }
        }

        public async Task<BatchGetSettingByKeyResponse> BatchGetByKeysAsync(BatchGetSettingByKeyRequest request)
        {
            var query = QueryHelpers.AddQueryString(
                String.Empty,
                request.Keys.Select(k => new KeyValuePair<string, string?>(nameof(request.Keys), k)));

            var response = await httpClient.GetFromJsonAsync($"settings/batch" + query, SourceGenerationContext.Default.BatchGetSettingByKeyResponse);

            ArgumentNullException.ThrowIfNull(response);

            return response;
        }

        public async Task BatchUpdateByKeysAsync(BatchUpdateSettingByKeyRequest request)
        {
            var response = await httpClient.PutAsync($"settings/batch", JsonContent.Create(request, SourceGenerationContext.Default.BatchUpdateSettingByKeyRequest));
            if (!response.IsSuccessStatusCode)
            {
                // TODO
            }
        }
    }
}
