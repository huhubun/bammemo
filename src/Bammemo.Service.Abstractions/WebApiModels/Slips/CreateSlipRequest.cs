namespace Bammemo.Service.Abstractions.WebApiModels.Slips;

public class CreateSlipRequest
{
    public required string Content { get; set; }
    public string? FriendlyUrl { get; set; }
    public SlipStatus Status { get; set; }
}
