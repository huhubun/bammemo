namespace Bammemo.Service.Abstractions.Dtos.Slips;

public class ListSlipQueryRequestDto
{
    public long? StartTime { get; set; }
    public long? EndTime { get; set; }
    public string[]? Tags { get; set; }
    public int[]? Status { get; set; }
}
