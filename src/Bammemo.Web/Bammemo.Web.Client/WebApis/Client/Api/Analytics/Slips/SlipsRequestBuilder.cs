// <auto-generated/>
#pragma warning disable CS0618
using Bammemo.Web.Client.WebApis.Client.Api.Analytics.Slips.Tags;
using Bammemo.Web.Client.WebApis.Client.Api.Analytics.Slips.Times;
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using System;
namespace Bammemo.Web.Client.WebApis.Client.Api.Analytics.Slips
{
    /// <summary>
    /// Builds and executes requests for operations under \api\analytics\slips
    /// </summary>
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    public partial class SlipsRequestBuilder : BaseRequestBuilder
    {
        /// <summary>The tags property</summary>
        public global::Bammemo.Web.Client.WebApis.Client.Api.Analytics.Slips.Tags.TagsRequestBuilder Tags
        {
            get => new global::Bammemo.Web.Client.WebApis.Client.Api.Analytics.Slips.Tags.TagsRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>The times property</summary>
        public global::Bammemo.Web.Client.WebApis.Client.Api.Analytics.Slips.Times.TimesRequestBuilder Times
        {
            get => new global::Bammemo.Web.Client.WebApis.Client.Api.Analytics.Slips.Times.TimesRequestBuilder(PathParameters, RequestAdapter);
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Bammemo.Web.Client.WebApis.Client.Api.Analytics.Slips.SlipsRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="pathParameters">Path parameters for the request</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public SlipsRequestBuilder(Dictionary<string, object> pathParameters, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/api/analytics/slips", pathParameters)
        {
        }
        /// <summary>
        /// Instantiates a new <see cref="global::Bammemo.Web.Client.WebApis.Client.Api.Analytics.Slips.SlipsRequestBuilder"/> and sets the default values.
        /// </summary>
        /// <param name="rawUrl">The raw URL to use for the request builder.</param>
        /// <param name="requestAdapter">The request adapter to use to execute the requests.</param>
        public SlipsRequestBuilder(string rawUrl, IRequestAdapter requestAdapter) : base(requestAdapter, "{+baseurl}/api/analytics/slips", rawUrl)
        {
        }
    }
}
#pragma warning restore CS0618
