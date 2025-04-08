namespace Bammemo.Web.WebApiModels.Slips;

public class ListSlipResponse
{
    public required SlipModel[] Data { get; set; }

    public class SlipModel
    {
        public required string Id { get; set; }
        public required string Content { get; set; }
        public long CreatedAt { get; set; }
        public long? UpdateAt { get; set; }
        public string? FriendlyLinkName { get; set; }
        public int Status { get; set; }
        public string? Title { get; set; }
        public string? Excerpt { get; set; }
        public string[]? Tags { get; set; }
        public SlipAttachmentModel[]? Attachments { get; set; }
    }

    public class SlipAttachmentModel
    {
        public int FileMetadataId { get; set; }
        public required string FileName { get; set; }
        public required string Url { get; set; }
        public bool ShowThumbnail { get; set; }
    }
}
