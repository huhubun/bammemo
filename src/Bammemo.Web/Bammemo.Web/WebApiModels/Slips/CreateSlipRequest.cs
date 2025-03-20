namespace Bammemo.Web.WebApiModels.Slips;

public class CreateSlipRequest
{
    public required string Content { get; set; }
    public SlipStatus Status { get; set; }
}
