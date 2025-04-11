using Bammemo.Service.Abstractions.SettingModels;
using Bammemo.Web.Client.Models.Settings;
using Bammemo.Web.Client.WebApis.Client;
using Bammemo.Web.Client.WebApis.Client.Models;
using Microsoft.FluentUI.AspNetCore.Components;
using System.Text.Json;

namespace Bammemo.Web.Client.Pages.Settings;

public partial class FunctionSetting(
    WebApiClient WebApiClient,
    IToastService ToastService)
{
    private static readonly string[] _defaultHighlightLanguages = [
        "bash",
        "csharp",
        "cshtml-razor",
        "css",
        "diff",
        "go",
        "http",
        "javascript",
        "json",
        "makefile",
        "markdown",
        "plaintext",
        "python",
        "shell",
        "sql",
        "typescript",
        "xml",
        "yaml"
    ];
    private readonly static string[] settingKeys = [
        SettingKeys.Highlight
    ];
    private FunctionSettingModel? model = null;
    private bool isSaving = false;

    protected override async Task OnInitializedAsync()
    {
        if (RendererInfo.Name != BlazorRendererName.WebAssembly)
        {
            return;
        }

        var response = await WebApiClient.Api.Settings.Batch.GetAsync(requestConfig => requestConfig.QueryParameters.Keys = settingKeys);

        model = new();

        foreach (var setting in response.Settings)
        {
            switch (setting.Key)
            {
                case SettingKeys.Highlight:
                    var highlightJsSetting = setting.Value != null ? JsonSerializer.Deserialize(setting.Value, JsonSourceGenerationContext.Default.FunctionHighlightSettingModel) : null;
                    model = highlightJsSetting?.MapTo(model);
                    break;
                default:
                    throw new NotSupportedException(setting.Key);
            }
        }
    }

    private async Task HandleValidSubmit()
    {
        isSaving = true;

        try
        {
            await WebApiClient.Api.Settings.Batch.PutAsync(new BatchUpdateSettingByKeyRequest
            {
                Settings = new BatchUpdateSettingByKeyRequest_settings
                {
                    AdditionalData = settingKeys.ToDictionary(key => key, key => key switch
                    {
                        SettingKeys.Highlight => JsonSerializer.Serialize(model.MapTo<FunctionHighlightSetting>(), JsonSourceGenerationContext.Default.FunctionHighlightSetting) as object,
                        _ => throw new NotSupportedException(key)
                    })
                }
            });

            ToastService.ShowSuccess("功能设置保存成功，刷新页面后生效。");
        }
        catch (Exception ex)
        {
            ToastService.ShowError($"保存失败：{ex.Message}");
            Console.WriteLine($"保存 function setting 失败：{ex}");
        }
        finally
        {
            isSaving = false;
        }
    }

    public class FunctionSettingModel
    {
        public string? HighlightCssLightUrl { get; set; }
        public string? HighlightCssDarkUrl { get; set; }
        public string? HighlightJsUrl { get; set; }
    }
}
