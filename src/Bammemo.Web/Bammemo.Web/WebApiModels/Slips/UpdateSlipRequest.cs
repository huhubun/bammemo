namespace Bammemo.Web.WebApiModels.Slips;

public class UpdateSlipRequest
{
    public required string Content { get; set; }
    public SlipStatus Status { get; set; }
    public List<AttachmentModel>? Attachments { get; set; }

    public class AttachmentModel
    {
        public int FileMetadataId { get; set; }
        public bool ShowThumbnail { get; set; }
    }
}
