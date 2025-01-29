namespace Bammemo.Service.Abstractions.Paginations;

public static class Extensions
{
    public static bool HasValue<T>(this CursorPagingRequest<T>? paging)
        => paging != null && paging.Take > 0 && paging.Cursor != null;

    public async static Task<CursorPagingRequest<int>?> DecodeAsync(
        this CursorPagingRequest<string>? paging,
        Func<string, Task<int>> decoder) => paging.HasValue() ? new CursorPagingRequest<int>
        {
            Cursor = await decoder(paging!.Cursor),
            Take = paging.Take
        } : null;

    public static Dictionary<string, string?> ToDictionary<T>(this CursorPagingRequest<T>? paging)
        => paging.HasValue() ? new Dictionary<string, string?>
        {
            {nameof(CursorPagingRequest<T>.Cursor), paging!.Cursor?.ToString() },
            {nameof(CursorPagingRequest<T>.Take), paging.Take.ToString() },
        } : [];

    public static List<KeyValuePair<string, string?>> ToQueryStringParameters<T>(this CursorPagingRequest<T>? paging)
        => paging.HasValue() ?
        [
            new KeyValuePair<string, string?>(nameof(CursorPagingRequest<T>.Cursor), paging!.Cursor?.ToString()),
            new KeyValuePair<string, string?>( nameof(CursorPagingRequest<T>.Take), paging.Take.ToString() ),
        ] : [];
}
