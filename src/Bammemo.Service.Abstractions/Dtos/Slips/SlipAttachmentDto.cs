namespace Bammemo.Service.Abstractions.Dtos.Slips;

public class SlipAttachmentDto
{
    public int FileMetadataId { get; set; }
    public required string FileName { get; set; }
    public required string Url { get; set; }
    public bool ShowThumbnail { get; set; }
}
