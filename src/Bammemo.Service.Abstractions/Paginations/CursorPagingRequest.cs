namespace Bammemo.Service.Abstractions.Paginations;

public class CursorPagingRequest<T> 
{
    public required T Cursor { get; set; }
    public required int Take { get; set; }
}
