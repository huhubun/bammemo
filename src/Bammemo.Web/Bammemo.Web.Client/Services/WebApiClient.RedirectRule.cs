using Bammemo.Service.Abstractions.WebApiModels.RedirectRules;
using System.Net.Http.Json;

namespace Bammemo.Web.Client.Services;

public partial class WebApiClient
{
    public class RedirectRuleClient(HttpClient httpClient)
    {
        public async Task<ListRedirectRuleResponse> ListAsync()
        {
            var response = await httpClient.GetFromJsonAsync($"redirectRules", SourceGenerationContext.Default.ListRedirectRuleResponse);

            ArgumentNullException.ThrowIfNull(response);

            return response;
        }

        public async Task<CreateRedirectRuleResponse?> CreateAsync(CreateRedirectRuleRequest request)
        {
            var responseMessage = await httpClient.PostAsJsonAsync($"redirectRules", request, SourceGenerationContext.Default.CreateRedirectRuleRequest);
            return await responseMessage.Content.ReadFromJsonAsync(SourceGenerationContext.Default.CreateRedirectRuleResponse);
        }

        public async Task UpdateAsync(int id, UpdateRedirectRuleRequest request)
            => await httpClient.PutAsync($"redirectRules/{id}", JsonContent.Create(request, SourceGenerationContext.Default.UpdateRedirectRuleRequest));

        public async Task DeleteAsync(int id)
            => await httpClient.DeleteAsync($"redirectRules/{id}");
    }
}
