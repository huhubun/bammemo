namespace Bammemo.Service.Abstractions.WebApiModels.Slips;

public class CreateSlipRequest
{
    public required string Content { get; set; }
    public SlipStatus Status { get; set; }
}
