namespace Bammemo.Service.Abstractions.Dtos;

public class SlipDto
{
    public required uint Id { get; set; }
    public required string Content { get; set; }
    public long CreatedAt { get; set; }
    public long? UpdateAt { get; set; }
    public string? FriendlyUrl { get; set; }
    public SlipStatus Status { get; set; }

    public ICollection<SlipTagDto>? Tags { get; set; }
}
