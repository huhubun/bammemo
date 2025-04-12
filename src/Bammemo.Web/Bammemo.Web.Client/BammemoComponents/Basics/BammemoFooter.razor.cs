using Bammemo.Service.Abstractions.CommonServices;
using Bammemo.Service.Abstractions.SettingModels;
using Microsoft.AspNetCore.Components;
using System.Collections.ObjectModel;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace Bammemo.Web.Client.BammemoComponents.Basics;

public partial class BammemoFooter(
    PersistentComponentState persistentComponentState,
    ICommonSettingService commonSettingService,
    NavigationManager navigationManager) : IDisposable
{
    #region GeneratedRegex

    [GeneratedRegex("^/login$", RegexOptions.IgnoreCase, 200)]
    private static partial Regex LoginPageRegex();

    [GeneratedRegex("^/about$", RegexOptions.IgnoreCase, 200)]
    private static partial Regex AboutPageRegex();

    [GeneratedRegex("^/links$", RegexOptions.IgnoreCase, 200)]
    private static partial Regex LinksPageRegex();

    [GeneratedRegex("^/tags", RegexOptions.IgnoreCase, 200)]
    private static partial Regex TagsPageRegex();

    [GeneratedRegex("^/settings(/.*)?$", RegexOptions.IgnoreCase, 200)]
    private static partial Regex SettingPageRegex();

    [GeneratedRegex("^/404", RegexOptions.IgnoreCase, 200)]
    private static partial Regex NotFoundPageRegex();

    #endregion

    private static readonly FooterGridItemDisplayType oneColumn = new()
    {
        Left = new(12)
    };
    private static readonly FooterGridItemDisplayType twoColumns = new()
    {
        Left = new(12, 8, 6),
        Right = new(null, 3, null)
    };
    private static readonly ReadOnlyCollection<Regex> oneColumnPageRegex = new([
        LoginPageRegex(),
        AboutPageRegex(),
        LinksPageRegex(),
        TagsPageRegex(),
        SettingPageRegex(),
        NotFoundPageRegex()
    ]);
    private static string DotNetVersion => System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;
    private static string RenderMode => "Blazor " + (OperatingSystem.IsBrowser() ? "WebAssembly" : "Server");

    private PersistingComponentStateSubscription persistingSubscription;
    private List<TextUrlSetting>? footerLinks;
    private string CurrentPageAbsolutePath => new Uri(navigationManager.Uri).AbsolutePath;
    private FooterGridItemDisplayType GridItemDisplayType => oneColumnPageRegex.Any(p => p.IsMatch(CurrentPageAbsolutePath)) ? oneColumn : twoColumns;

    protected override async Task OnInitializedAsync()
    {
        persistingSubscription = persistentComponentState.RegisterOnPersisting(PersistData);

        if (!persistentComponentState.TryTakeFromJson(nameof(footerLinks), out footerLinks))
        {
            var setting = await commonSettingService.GetByKeyAsync(SettingKeys.FooterLinks);
            footerLinks = setting?.Value != null ? JsonSerializer.Deserialize<List<TextUrlSetting>>(setting.Value, JsonSourceGenerationContext.Default.ListTextUrlSetting) : [];
        }
    }

    private Task PersistData()
    {
        persistentComponentState.PersistAsJson(nameof(footerLinks), footerLinks);
        return Task.CompletedTask;
    }

    void IDisposable.Dispose() => persistingSubscription.Dispose();

    record FooterGridItemDisplayType
    {
        public required FooterGridItemSpan Left { get; init; }
        public FooterGridItemSpan? Right { get; init; }
    }

    record FooterGridItemSpan
    {
        public FooterGridItemSpan(int? xs)
        {
            Xs = xs;
        }

        public FooterGridItemSpan(int? xs, int? lg, int? xl)
        {
            Xs = xs;
            Lg = lg;
            Xl = xl;
        }

        public int? Xs { get; init; }
        public int? Lg { get; init; }
        public int? Xl { get; init; }
    }
}
