using Bammemo.Service.Abstractions.WebApiModels.SiteLinks;
using System.Net.Http.Json;

namespace Bammemo.Web.Client.Services;

public partial class WebApiClient
{
    public class SiteLinkClient(HttpClient httpClient)
    {
        public async Task<ListSiteLinkResponse> ListAsync()
        {
            var response = await httpClient.GetFromJsonAsync($"siteLinks", SourceGenerationContext.Default.ListSiteLinkResponse);

            ArgumentNullException.ThrowIfNull(response);

            return response;
        }

        public async Task<CreateSiteLinkResponse?> CreateAsync(CreateSiteLinkRequest request)
        {
            var responseMessage = await httpClient.PostAsJsonAsync($"siteLinks", request, SourceGenerationContext.Default.CreateSiteLinkRequest);
            return await responseMessage.Content.ReadFromJsonAsync<CreateSiteLinkResponse>(SourceGenerationContext.Default.CreateSiteLinkResponse);
        }

        public async Task UpdateAsync(int id, UpdateSiteLinkRequest request)
        {
            await httpClient.PutAsync($"siteLinks/{id}", JsonContent.Create(request, SourceGenerationContext.Default.UpdateSiteLinkRequest));
        }

        public async Task DeleteAsync(int id)
            => await httpClient.DeleteAsync($"siteLinks/{id}");
    }
}
