namespace Bammemo.Web.WebApiModels.Slips;

public class CreateSlipResponse
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
}
