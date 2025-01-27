using Bammemo.Web.Client.Layout;
using Microsoft.AspNetCore.Components;

namespace Bammemo.Web.Components.Layout;

internal class ServerStaticAssetService : IStaticAssetService
{
    private readonly HttpClient _httpClient;

    public ServerStaticAssetService(HttpClient httpClient, NavigationManager navigationManager)
    {
        _httpClient = httpClient;
        _httpClient.BaseAddress ??= new Uri(navigationManager.BaseUri);
    }
    public async Task<string?> GetAsync(string assetUrl, bool useCache = true)
    {
        var message = new HttpRequestMessage(HttpMethod.Get, assetUrl);
        var response = await _httpClient.SendAsync(message);

        return await response.Content.ReadAsStringAsync();
    }
}
