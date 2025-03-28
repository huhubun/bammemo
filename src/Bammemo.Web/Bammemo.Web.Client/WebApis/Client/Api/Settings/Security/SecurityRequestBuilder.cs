// <auto-generated/>
#pragma warning disable CS0618
using Bammemo.Web.Client.WebApis.Client.Api.Settings.Security.KeySource;
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
namespace Bammemo.Web.Client.WebApis.Client.Api.Settings.Security
{
    /// <summary>
    /// Builds and executes requests for operations under \api\settings\security
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class SecurityRequestBuilder : BaseRequestBuilder
    {
        /// <summary>The keySource property</summary>
        public global::Bammemo.Web.Client.WebApis.Client.Api.Settings.Security.KeySource.KeySourceRequestBuilder KeySource
        {
            get => new global::Bammemo.Web.Client.WebApis.Client.Api.Settings.Security.KeySource.KeySourceRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Bammemo.Web.Client.WebApis.Client.Api.Settings.Security.SecurityRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public SecurityRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/api/settings/security", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Bammemo.Web.Client.WebApis.Client.Api.Settings.Security.SecurityRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public SecurityRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/api/settings/security", rawUrl)
        {
        }
    }
}
#pragma warning restore CS0618
