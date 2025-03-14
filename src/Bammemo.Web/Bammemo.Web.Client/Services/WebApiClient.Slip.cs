﻿using Bammemo.Service.Abstractions.Paginations;
using Bammemo.Service.Abstractions.WebApiModels.Slips;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Json;

namespace Bammemo.Web.Client.Services;

public partial class WebApiClient
{
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
            var response = await httpClient.GetFromJsonAsync("slips" + queryString, SourceGenerationContext.Default.ListSlipResponse);

            return response;
        }

        public async Task<GetSlipByIdResponse?> GetByIdAsync(string idOrLinkName, GetSlipByIdRequest? request = null)
        {
            var queryString = request?.Type != null ? QueryHelpers.AddQueryString(String.Empty, nameof(request.Type), request.Type.Value.ToString()) : null;
            return await httpClient.GetFromJsonAsync($"slips/{idOrLinkName}" + queryString, SourceGenerationContext.Default.GetSlipByIdResponse);
        }

        public async Task<CreateSlipResponse?> CreateAsync(CreateSlipRequest request)
        {
            var responseMessage = await httpClient.PostAsJsonAsync("slips", request, SourceGenerationContext.Default.CreateSlipRequest);
            var response = await responseMessage.Content.ReadFromJsonAsync(SourceGenerationContext.Default.CreateSlipResponse);

            return response;
        }

        public async Task<UpdateSlipResponse?> UpdateAsync(string id, UpdateSlipRequest request)
        {
            var responseMessage = await httpClient.PutAsJsonAsync($"slips/{id}", request, SourceGenerationContext.Default.UpdateSlipRequest);
            var response = await responseMessage.Content.ReadFromJsonAsync(SourceGenerationContext.Default.UpdateSlipResponse);

            return response;
        }

        public async Task DeleteAsync(string id)
            => await httpClient.DeleteAsync($"slips/{id}");
    }
}
