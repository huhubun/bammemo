// <auto-generated/>
#pragma warning disable CS0618
using Bammemo.Web.Client.WebApis.Client.Api.Settings.Batch;
using Bammemo.Web.Client.WebApis.Client.Api.Settings.Item;
using Bammemo.Web.Client.WebApis.Client.Api.Settings.Security;
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
namespace Bammemo.Web.Client.WebApis.Client.Api.Settings
{
    /// <summary>
    /// Builds and executes requests for operations under \api\settings
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class SettingsRequestBuilder : BaseRequestBuilder
    {
        /// <summary>The batch property</summary>
        public global::Bammemo.Web.Client.WebApis.Client.Api.Settings.Batch.BatchRequestBuilder Batch
        {
            get => new global::Bammemo.Web.Client.WebApis.Client.Api.Settings.Batch.BatchRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The security property</summary>
        public global::Bammemo.Web.Client.WebApis.Client.Api.Settings.Security.SecurityRequestBuilder Security
        {
            get => new global::Bammemo.Web.Client.WebApis.Client.Api.Settings.Security.SecurityRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>Gets an item from the Bammemo.Web.Client.WebApis.Client.api.settings.item collection</summary>
        /// <param name="position">Unique identifier of the item</param>
        /// <returns>A <see cref="global::Bammemo.Web.Client.WebApis.Client.Api.Settings.Item.WithKeyItemRequestBuilder"/></returns>
        public global::Bammemo.Web.Client.WebApis.Client.Api.Settings.Item.WithKeyItemRequestBuilder this[string position]
        {
            get
            {
                var urlTplParams = new Dictionary<string, object>(PathParameters);
                urlTplParams.Add("key", position);
                return new global::Bammemo.Web.Client.WebApis.Client.Api.Settings.Item.WithKeyItemRequestBuilder(urlTplParams, RequestAdapter);
            }
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Bammemo.Web.Client.WebApis.Client.Api.Settings.SettingsRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public SettingsRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/api/settings", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Bammemo.Web.Client.WebApis.Client.Api.Settings.SettingsRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public SettingsRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/api/settings", rawUrl)
        {
        }
    }
}
#pragma warning restore CS0618
