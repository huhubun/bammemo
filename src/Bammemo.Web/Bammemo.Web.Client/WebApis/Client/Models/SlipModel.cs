// <auto-generated/>
#pragma warning disable CS0618
using Microsoft.Kiota.Abstractions.Extensions;
using Microsoft.Kiota.Abstractions.Serialization;
using System.Collections.Generic;
using System.IO;
using System;
namespace Bammemo.Web.Client.WebApis.Client.Models
{
    [global::System.CodeDom.Compiler.GeneratedCode("Kiota", "1.0.0")]
    #pragma warning disable CS1591
    public partial class SlipModel : IAdditionalDataHolder, IParsable
    #pragma warning restore CS1591
    {
        /// <summary>Stores additional data not described in the OpenAPI description found when deserializing. Can be used for serialization as well.</summary>
        public IDictionary<string, object> AdditionalData { get; set; }
        /// <summary>The attachments property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<global::Bammemo.Web.Client.WebApis.Client.Models.SlipAttachmentModel>? Attachments { get; set; }
#nullable restore
#else
        public List<global::Bammemo.Web.Client.WebApis.Client.Models.SlipAttachmentModel> Attachments { get; set; }
#endif
        /// <summary>The content property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Content { get; set; }
#nullable restore
#else
        public string Content { get; set; }
#endif
        /// <summary>The createdAt property</summary>
        public long? CreatedAt { get; set; }
        /// <summary>The excerpt property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Excerpt { get; set; }
#nullable restore
#else
        public string Excerpt { get; set; }
#endif
        /// <summary>The friendlyLinkName property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? FriendlyLinkName { get; set; }
#nullable restore
#else
        public string FriendlyLinkName { get; set; }
#endif
        /// <summary>The id property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Id { get; set; }
#nullable restore
#else
        public string Id { get; set; }
#endif
        /// <summary>The status property</summary>
        public int? Status { get; set; }
        /// <summary>The tags property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public List<string>? Tags { get; set; }
#nullable restore
#else
        public List<string> Tags { get; set; }
#endif
        /// <summary>The title property</summary>
#if NETSTANDARD2_1_OR_GREATER || NETCOREAPP3_1_OR_GREATER
#nullable enable
        public string? Title { get; set; }
#nullable restore
#else
        public string Title { get; set; }
#endif
        /// <summary>The updateAt property</summary>
        public long? UpdateAt { get; set; }
        /// <summary>
        /// Instantiates a new <see cref="global::Bammemo.Web.Client.WebApis.Client.Models.SlipModel"/> and sets the default values.
        /// </summary>
        public SlipModel()
        {
            AdditionalData = new Dictionary<string, object>();
        }
        /// <summary>
        /// Creates a new instance of the appropriate class based on discriminator value
        /// </summary>
        /// <returns>A <see cref="global::Bammemo.Web.Client.WebApis.Client.Models.SlipModel"/></returns>
        /// <param name="parseNode">The parse node to use to read the discriminator value and create the object</param>
        public static global::Bammemo.Web.Client.WebApis.Client.Models.SlipModel CreateFromDiscriminatorValue(IParseNode parseNode)
        {
            _ = parseNode ?? throw new ArgumentNullException(nameof(parseNode));
            return new global::Bammemo.Web.Client.WebApis.Client.Models.SlipModel();
        }
        /// <summary>
        /// The deserialization information for the current model
        /// </summary>
        /// <returns>A IDictionary&lt;string, Action&lt;IParseNode&gt;&gt;</returns>
        public virtual IDictionary<string, Action<IParseNode>> GetFieldDeserializers()
        {
            return new Dictionary<string, Action<IParseNode>>
            {
                { "attachments", n => { Attachments = n.GetCollectionOfObjectValues<global::Bammemo.Web.Client.WebApis.Client.Models.SlipAttachmentModel>(global::Bammemo.Web.Client.WebApis.Client.Models.SlipAttachmentModel.CreateFromDiscriminatorValue)?.AsList(); } },
                { "content", n => { Content = n.GetStringValue(); } },
                { "createdAt", n => { CreatedAt = n.GetLongValue(); } },
                { "excerpt", n => { Excerpt = n.GetStringValue(); } },
                { "friendlyLinkName", n => { FriendlyLinkName = n.GetStringValue(); } },
                { "id", n => { Id = n.GetStringValue(); } },
                { "status", n => { Status = n.GetIntValue(); } },
                { "tags", n => { Tags = n.GetCollectionOfPrimitiveValues<string>()?.AsList(); } },
                { "title", n => { Title = n.GetStringValue(); } },
                { "updateAt", n => { UpdateAt = n.GetLongValue(); } },
            };
        }
        /// <summary>
        /// Serializes information the current object
        /// </summary>
        /// <param name="writer">Serialization writer to use to serialize this model</param>
        public virtual void Serialize(ISerializationWriter writer)
        {
            _ = writer ?? throw new ArgumentNullException(nameof(writer));
            writer.WriteCollectionOfObjectValues<global::Bammemo.Web.Client.WebApis.Client.Models.SlipAttachmentModel>("attachments", Attachments);
            writer.WriteStringValue("content", Content);
            writer.WriteLongValue("createdAt", CreatedAt);
            writer.WriteStringValue("excerpt", Excerpt);
            writer.WriteStringValue("friendlyLinkName", FriendlyLinkName);
            writer.WriteStringValue("id", Id);
            writer.WriteIntValue("status", Status);
            writer.WriteCollectionOfPrimitiveValues<string>("tags", Tags);
            writer.WriteStringValue("title", Title);
            writer.WriteLongValue("updateAt", UpdateAt);
            writer.WriteAdditionalData(AdditionalData);
        }
    }
}
#pragma warning restore CS0618
