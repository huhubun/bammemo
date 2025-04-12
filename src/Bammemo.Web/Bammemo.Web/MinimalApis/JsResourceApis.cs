using Bammemo.Service.Abstractions.SettingModels;
using Bammemo.Service.Helpers;
using Bammemo.Service.Interfaces;
using Bammemo.Service.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using System.Net.Mime;
using System.Text.Json;

namespace Bammemo.Web.MinimalApis;

public static class JsResourceApis
{
    private const string DEFAULT_HIGHLIGHT_CSS_LIGHT_PATH = "/styles/highlight/github.min.css";
    private const string DEFAULT_HIGHLIGHT_CSS_DARK_PATH = "/styles/highlight/github-dark.min.css";
    private const string DEFAULT_HIGHLIGHT_JS_PATH = "/js/highlight.min.js";

    public static WebApplication MapJsResourceApi(this WebApplication app)
    {
        app.MapGet("/js/highlight-extensions.js", HighlightExtensionsAsync).ExcludeFromDescription();

        return app;
    }

    private static async Task<IResult> HighlightExtensionsAsync(
        [FromServices] ISettingService settingService,
        [FromServices] IMemoryCache memoryCache,
        [FromServices] IWebHostEnvironment webHostEnvironment,
        HttpContext context)
    {
        FunctionHighlightSetting? highlightSetting;

        var highlightSettingString = await settingService.GetByKeyFromCacheAsync(SettingKeys.Highlight);
        if (highlightSettingString == null || highlightSettingString.Value == null)
        {
            highlightSetting = null;
        }
        else
        {
            highlightSetting = JsonSerializer.Deserialize<FunctionHighlightSetting>(
                highlightSettingString.Value,
                BammemoJsonSerializerOptions.CamelCaseOption);
        }

        var defaultHighlight = memoryCache.GetOrCreate($"{nameof(JsResourceApis)}-default-highlight", _ => new List<string>
            {
                DEFAULT_HIGHLIGHT_CSS_LIGHT_PATH,
                DEFAULT_HIGHLIGHT_CSS_DARK_PATH,
                DEFAULT_HIGHLIGHT_JS_PATH
            }.ToDictionary(i => i, i =>
            {
                var filePath = Path.Combine(webHostEnvironment.WebRootPath, i.TrimStart('/'));
                var (algorithm, hash) = HashHelper.Sha384(filePath);

                return $"{algorithm.ToLowerInvariant()}-{hash}";
            }));

        var (cssLight, cssDark, js) = await memoryCache.GetOrCreateAsync($"{nameof(JsResourceApis)}-setting-highlight", async _ =>
        {
            var cssLightTask = GetUrlAndIntegrityAsync(highlightSetting?.HighlightCssLightUrl);
            var cssDarkTask = GetUrlAndIntegrityAsync(highlightSetting?.HighlightCssDarkUrl);
            var jsTask = GetUrlAndIntegrityAsync(highlightSetting?.HighlightJsUrl);

            await Task.WhenAll(cssLightTask, cssDarkTask, jsTask);

            return (cssLight: cssLightTask.Result, cssDark: cssDarkTask.Result, js: jsTask.Result);
        });

        var script = $$"""
        // Add Stylesheets
        hljs_addStylesheet('{{cssLight?.url ?? DEFAULT_HIGHLIGHT_CSS_LIGHT_PATH}}', 'highlight-light', null, '{{cssLight?.integrity ?? defaultHighlight![DEFAULT_HIGHLIGHT_CSS_LIGHT_PATH]}}');
        hljs_addStylesheet('{{cssDark?.url ?? DEFAULT_HIGHLIGHT_CSS_DARK_PATH}}', 'highlight-dark', 'disabled', '{{cssDark?.integrity ?? defaultHighlight![DEFAULT_HIGHLIGHT_CSS_DARK_PATH]}}');

        hljs_addInlineStylesheet(`pre[class~="snippet"] {
            --font-monospace: "courier";
            --type-ramp-base-font-variations: unset;
            font-weight: bold;
            }`);

        // Add Scripts
        const highlight = hljs_addJavaScript('{{js?.url ?? DEFAULT_HIGHLIGHT_JS_PATH}}', '{{js?.integrity ?? defaultHighlight![DEFAULT_HIGHLIGHT_JS_PATH]}}');

        // Add custom code
        highlight.onload = () => {
            // Switch highlight Dark/Light theme
            const theme = document.querySelector('loading-theme > fluent-design-theme');
            if (theme != null) {
                theme.addEventListener('onchange', (e) => {
                    if (e.detail.name == 'mode') {
                        if (e.detail.newValue === 'undefined') return;
                        const isDark =  e.detail.newValue.includes('dark');
                        hljs_ColorSwitcher(isDark);
                    }
                });
            }

            // Detect system theme changing
            window.matchMedia('(prefers-color-scheme: dark)')
                .addEventListener('change', (e) => {
                    hljs_ColorSystem();
                });

            // First/default theme
            hljs_ColorSystem();
        }
        function hljs_ColorSystem() {
            const theme = document.querySelector('loading-theme > fluent-design-theme');
            if (theme != null) {
                if (theme.getAttribute('mode') == 'null' || theme.getAttribute('mode') == null || theme.getAttribute('mode').value === undefined) {
                    const isSystemDark = window.matchMedia && window.matchMedia('(prefers-color-scheme: dark)').matches;
                    hljs_ColorSwitcher(isSystemDark);
                }
            }
        }

        function hljs_ColorSwitcher(isDark) {
            const darkCss = document.querySelector('link[title="highlight-dark"]');
            const lightCss = document.querySelector('link[title="highlight-light"]');

            if (isDark) {
                darkCss.removeAttribute("disabled");
                lightCss.setAttribute("disabled", "disabled");
            }
            else {
                lightCss.removeAttribute("disabled");
                darkCss.setAttribute("disabled", "disabled");
            }
        }

        // Add a <script> to the <body> element
        function hljs_addJavaScript(src, integrity) {
            const script = document.createElement('script');
            script.type = 'text/javascript';
            script.src = src;
            script.async = true;

            if(integrity) {
                script.integrity = integrity;
                script.crossOrigin = 'anonymous';
            }

            script.onerror = () => {
                // Error occurred while loading script
                console.error('Error occurred while loading script', src);
            };

            document.body.appendChild(script);

            return script;
        }

        // Add a <link> to the <head> element
        function hljs_addStylesheet(src, title, disabled, integrity) {
            const stylesheet = document.createElement('link');
            stylesheet.rel = 'stylesheet';
            stylesheet.href = src;
            if (title) stylesheet.title = title;
            if (disabled) stylesheet.disabled = disabled;

            if(integrity) {
                stylesheet.integrity = integrity;
                stylesheet.crossOrigin = 'anonymous';
            }

            stylesheet.onerror = () => {
                // Error occurred while loading stylesheet
                console.error('Error occurred while loading stylesheet', src);
            };

            document.head.appendChild(stylesheet);

            return stylesheet;
        }

        function hljs_addInlineStylesheet(code) {
            const stylesheet = document.createElement('style');
            stylesheet.innerText = code;

            document.head.appendChild(stylesheet);

            return stylesheet;
        }
        """;

        return Results.Text(script, MediaTypeNames.Text.JavaScript);
    }

    private async static Task<(string? url, string? integrity)?> GetUrlAndIntegrityAsync(string? url)
    {
        if (!String.IsNullOrWhiteSpace(url))
        {
            using var httpClient = new HttpClient();
            using var stream = await httpClient.GetStreamAsync(url);

            var (algorithm, hash) = HashHelper.Sha384(stream);

            return (url, $"{algorithm.ToLowerInvariant()}-{hash}");
        }

        return null;
    }
}
