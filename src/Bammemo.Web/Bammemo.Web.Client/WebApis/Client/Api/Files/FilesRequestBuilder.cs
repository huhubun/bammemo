// <auto-generated/>
#pragma warning disable CS0618
using Bammemo.Web.Client.WebApis.Client.Api.Files.Item;
using Bammemo.Web.Client.WebApis.Client.Models;
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using Microsoft.Kiota.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System.Threading;
using System;
namespace Bammemo.Web.Client.WebApis.Client.Api.Files
{
    /// <summary>
    /// Builds and executes requests for operations under \api\files
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class FilesRequestBuilder : BaseRequestBuilder
    {
        /// <summary>Gets an item from the Bammemo.Web.Client.WebApis.Client.api.files.item collection</summary>
        /// <param name="position">Unique identifier of the item</param>
        /// <returns>A <see cref="global::Bammemo.Web.Client.WebApis.Client.Api.Files.Item.WithFileMetadataItemRequestBuilder"/></returns>
        public global::Bammemo.Web.Client.WebApis.Client.Api.Files.Item.WithFileMetadataItemRequestBuilder this[int position]
        {
            get
            {
                var urlTplParams = new Dictionary<string, object>(PathParameters);
                urlTplParams.Add("fileMetadataId", position);
                return new global::Bammemo.Web.Client.WebApis.Client.Api.Files.Item.WithFileMetadataItemRequestBuilder(urlTplParams, RequestAdapter);
            }
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Bammemo.Web.Client.WebApis.Client.Api.Files.FilesRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public FilesRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/api/files", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Bammemo.Web.Client.WebApis.Client.Api.Files.FilesRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public FilesRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/api/files", rawUrl)
        {
        }
        /// <returns>A <see cref="global::Bammemo.Web.Client.WebApis.Client.Models.UploadFileResponse"/></returns>
        /// <param name="body">The request body</param>
        /// <param name="cancellationToken">Cancellation token to use when cancelling requests</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public async Task<global::Bammemo.Web.Client.WebApis.Client.Models.UploadFileResponse?> PostAsync(MultipartBody body, Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#nullable restore
#else
        public async Task<global::Bammemo.Web.Client.WebApis.Client.Models.UploadFileResponse> PostAsync(MultipartBody body, Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default, CancellationToken cancellationToken = default)
        {
#endif
            _ = body ?? throw new ArgumentNullException(nameof(body));
            var requestInfo = ToPostRequestInformation(body, requestConfiguration);
            return await RequestAdapter.SendAsync<global::Bammemo.Web.Client.WebApis.Client.Models.UploadFileResponse>(requestInfo, global::Bammemo.Web.Client.WebApis.Client.Models.UploadFileResponse.CreateFromDiscriminatorValue, default, cancellationToken).ConfigureAwait(false);
        }
        /// <returns>A <see cref="RequestInformation"/></returns>
        /// <param name="body">The request body</param>
        /// <param name="requestConfiguration">Configuration for the request such as headers, query parameters, and middleware options.</param>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public RequestInformation ToPostRequestInformation(MultipartBody body, Action<RequestConfiguration<DefaultQueryParameters>>? requestConfiguration = default)
        {
#nullable restore
#else
        public RequestInformation ToPostRequestInformation(MultipartBody body, Action<RequestConfiguration<DefaultQueryParameters>> requestConfiguration = default)
        {
#endif
            _ = body ?? throw new ArgumentNullException(nameof(body));
            var requestInfo = new RequestInformation(Method.POST, UrlTemplate, PathParameters);
            requestInfo.Configure(requestConfiguration);
            requestInfo.Headers.TryAdd("Accept", "application/json, text/plain;q=0.9");
            requestInfo.SetContentFromParsable(RequestAdapter, "multipart/form-data", body);
            return requestInfo;
        }
        /// <summary>
        /// Returns a request builder with the provided arbitrary URL. Using this method means any other path or query parameters are ignored.
        /// </summary>
        /// <returns>A <see cref="global::Bammemo.Web.Client.WebApis.Client.Api.Files.FilesRequestBuilder"/></returns>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        public global::Bammemo.Web.Client.WebApis.Client.Api.Files.FilesRequestBuilder WithUrl(string rawUrl)
        {
            return new global::Bammemo.Web.Client.WebApis.Client.Api.Files.FilesRequestBuilder(rawUrl, RequestAdapter);
        }
    }
}
#pragma warning restore CS0618
