namespace Bammemo.Web.WebApiModels.Slips;

public class UpdateSlipRequest
{
    public required string Content { get; set; }
    public SlipStatus Status { get; set; }
}
