namespace Bammemo.Service.Abstractions.WebApiModels.Slips;

public class GetSlipTimesRequest
{
    public long StartTime { get; set; }
    public long EndTime { get; set; }
}
