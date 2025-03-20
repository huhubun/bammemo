// <auto-generated/>
#pragma warning disable CS0618
using Bammemo.Web.Client.WebApis.Client.Api.RedirectRules;
using Bammemo.Web.Client.WebApis.Client.Api.Settings;
using Bammemo.Web.Client.WebApis.Client.Api.Slips;
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
namespace Bammemo.Web.Client.WebApis.Client.Api
{
    /// <summary>
    /// Builds and executes requests for operations under \api
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class ApiRequestBuilder : BaseRequestBuilder
    {
        /// <summary>The redirectRules property</summary>
        public global::Bammemo.Web.Client.WebApis.Client.Api.RedirectRules.RedirectRulesRequestBuilder RedirectRules
        {
            get => new global::Bammemo.Web.Client.WebApis.Client.Api.RedirectRules.RedirectRulesRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The settings property</summary>
        public global::Bammemo.Web.Client.WebApis.Client.Api.Settings.SettingsRequestBuilder Settings
        {
            get => new global::Bammemo.Web.Client.WebApis.Client.Api.Settings.SettingsRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The slips property</summary>
        public global::Bammemo.Web.Client.WebApis.Client.Api.Slips.SlipsRequestBuilder Slips
        {
            get => new global::Bammemo.Web.Client.WebApis.Client.Api.Slips.SlipsRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Bammemo.Web.Client.WebApis.Client.Api.ApiRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public ApiRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/api", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Bammemo.Web.Client.WebApis.Client.Api.ApiRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public ApiRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/api", rawUrl)
        {
        }
    }
}
#pragma warning restore CS0618
