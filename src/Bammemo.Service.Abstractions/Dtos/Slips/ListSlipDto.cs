﻿namespace Bammemo.Service.Abstractions.Dtos.Slips;

public class ListSlipDto
{
    public string? Id { get; set; }
    public required string Content { get; set; }
    public long CreatedAt { get; set; }
    public long? UpdateAt { get; set; }
    public string? FriendlyLinkName { get; set; }
    public SlipStatus Status { get; set; }
    public string? Title { get; set; }
    public string? Excerpt { get; set; }
    public SlipAttachmentDto[]? Attachments { get; set; }
}
