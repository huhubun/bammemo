using Bammemo.Service.Abstractions.SettingModels;
using Bammemo.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Text.Json;

namespace Bammemo.Web.MinimalApis;

public static class JsResourceApis
{
    public static WebApplication MapJsResourceApi(this WebApplication app)
    {
        app.MapGet("/js/highlight-extensions.js", HighlightExtensionsAsync).ExcludeFromDescription();

        return app;
    }

    private static async Task<IResult> HighlightExtensionsAsync(
        [FromServices] ISettingService settingService,
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
                new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
        }

        var cssLightUrl = String.IsNullOrWhiteSpace(highlightSetting?.HighlightCssLightUrl) ? "/styles/highlight/github.min.css" : highlightSetting.HighlightCssLightUrl;
        var cssDarkUrl = String.IsNullOrWhiteSpace(highlightSetting?.HighlightCssDarkUrl) ? "/styles/highlight/github-dark.min.css" : highlightSetting.HighlightCssDarkUrl;
        var jsUrl = String.IsNullOrWhiteSpace(highlightSetting?.HighlightJsUrl) ? "/js/highlight.min.js" : highlightSetting.HighlightJsUrl;

        var script = $$"""
        // Add Stylesheets
        hljs_addStylesheet('{{cssLightUrl}}', 'highlight-light', null);
        hljs_addStylesheet('{{cssDarkUrl}}', 'highlight-dark', 'disabled');

        hljs_addInlineStylesheet(`pre[class~="snippet"] {
            --font-monospace: "courier";
            --type-ramp-base-font-variations: unset;
            font-weight: bold;
            }`);

        // Add Scripts
        const highlight = hljs_addJavaScript('{{jsUrl}}');

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
        function hljs_addJavaScript(src) {
            const script = document.createElement('script');
            script.type = 'text/javascript';
            script.src = src;
            script.async = true;

            script.onerror = () => {
                // Error occurred while loading script
                console.error('Error occurred while loading script', src);
            };

            document.body.appendChild(script);

            return script;
        }

        // Add a <link> to the <head> element
        function hljs_addStylesheet(src, title, disabled) {
            const stylesheet = document.createElement('link');
            stylesheet.rel = 'stylesheet';
            stylesheet.href = src;
            if (title) stylesheet.title = title;
            if (disabled) stylesheet.disabled = disabled;

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
}
