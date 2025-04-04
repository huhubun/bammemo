// <auto-generated/>
#pragma warning disable CS0618
using Bammemo.Web.Client.WebApis.Client.Api.Analytics.Slips;
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
namespace Bammemo.Web.Client.WebApis.Client.Api.Analytics
{
    /// <summary>
    /// Builds and executes requests for operations under \api\analytics
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class AnalyticsRequestBuilder : BaseRequestBuilder
    {
        /// <summary>The slips property</summary>
        public global::Bammemo.Web.Client.WebApis.Client.Api.Analytics.Slips.SlipsRequestBuilder Slips
        {
            get => new global::Bammemo.Web.Client.WebApis.Client.Api.Analytics.Slips.SlipsRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Bammemo.Web.Client.WebApis.Client.Api.Analytics.AnalyticsRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public AnalyticsRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/api/analytics", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Bammemo.Web.Client.WebApis.Client.Api.Analytics.AnalyticsRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public AnalyticsRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/api/analytics", rawUrl)
        {
        }
    }
}
#pragma warning restore CS0618
