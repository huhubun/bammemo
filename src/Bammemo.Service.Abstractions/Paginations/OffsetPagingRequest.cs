namespace Bammemo.Service.Abstractions.Paginations;

public class OffsetPagingRequest
{
    public required int Offset { get; set; }
    public required int Take { get; set; }
}
