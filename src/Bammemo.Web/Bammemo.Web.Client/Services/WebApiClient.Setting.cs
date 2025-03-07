using Bammemo.Service.Abstractions.WebApiModels.Settings;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;

namespace Bammemo.Web.Client.Services;

public partial class WebApiClient
{
    public class SettingClient(HttpClient httpClient)
    {
        public async Task<GetSettingByKeyResponse> GetByKeyAsync(string key)
        {
            var response = await httpClient.GetFromJsonAsync<GetSettingByKeyResponse>($"settings/{key}");

            ArgumentNullException.ThrowIfNull(response);

            return response;
        }

        public async Task<BatchGetSettingByKeyResponse> BatchGetByKeysAsync(BatchGetSettingByKeyRequest request)
        {
            var query = QueryHelpers.AddQueryString(
                String.Empty,
                request.Keys.Select(k => new KeyValuePair<string, string?>(nameof(request.Keys), k)));

            var response = await httpClient.GetFromJsonAsync<BatchGetSettingByKeyResponse>($"settings/batch" + query);

            ArgumentNullException.ThrowIfNull(response);

            return response;
        }

        public async Task BatchUpdateByKeysAsync(BatchUpdateSettingByKeyRequest request)
        {
            var response = await httpClient.PutAsync($"settings/batch", JsonContent.Create(request));
            if (!response.IsSuccessStatusCode)
            {
                // TODO
            }
        }
    }
}
