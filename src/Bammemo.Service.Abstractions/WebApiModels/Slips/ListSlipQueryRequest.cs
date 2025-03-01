namespace Bammemo.Service.Abstractions.WebApiModels.Slips;

public class ListSlipQueryRequest
{
    public long? StartTime { get; set; }
    public long? EndTime { get; set; }
    public string[]? Tags { get; set; }
}
