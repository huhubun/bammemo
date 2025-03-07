namespace Bammemo.Service.Abstractions
{
    // https://learn.microsoft.com/zh-cn/aspnet/core/blazor/components/render-modes?view=aspnetcore-9.0#detect-rendering-location-interactivity-and-assigned-render-mode-at-runtime
    public static class BlazorRendererName
    {
        public const string Static = nameof(Static);
        public const string Server = nameof(Server);
        public const string WebAssembly = nameof(WebAssembly);
        public const string WebView = nameof(WebView);
    }
}
