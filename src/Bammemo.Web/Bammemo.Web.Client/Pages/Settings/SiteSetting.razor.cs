using Bammemo.Service.Abstractions.Enums;
using Bammemo.Web.Client.WebApis.Client;
using Microsoft.FluentUI.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using System.Net.Mime;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Bammemo.Web.Client.Pages.Settings;

public partial class SiteSetting(
    WebApiClient WebApiClient,
    IToastService ToastService)
{
    private readonly static string[] settingKeys = [
        SettingKeys.SiteName,
        SettingKeys.SiteDescription,
        SettingKeys.SiteLogoUrl,
        SettingKeys.FaviconUrl,
        SettingKeys.SiteKeywords,
        SettingKeys.FooterLinks
    ];
    private string? inputedKeyword;
    private SiteSettingModel? model = null;
    private bool isSaving = false;
    private bool isSiteLogoUploading = false;
    private bool isFaviconUploading = false;

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
                case SettingKeys.SiteName:
                    model.SiteName = setting.Value;
                    break;
                case SettingKeys.SiteDescription:
                    model.SiteDescription = setting.Value;
                    break;
                case SettingKeys.SiteLogoUrl:
                    model.SiteLogoUrl = setting.Value;
                    break;
                case SettingKeys.FaviconUrl:
                    model.FaviconUrl = setting.Value;
                    break;
                case SettingKeys.SiteKeywords:
                    model.SiteKeywords = setting.Value != null ? JsonSerializer.Deserialize(setting.Value, JsonSourceGenerationContext.Default.ListString) : [];
                    break;
                case SettingKeys.FooterLinks:
                    model.FooterLinks = setting.Value != null ? JsonSerializer.Deserialize(setting.Value, JsonSourceGenerationContext.Default.ListTextUrlModel) : [];
                    break;
                default:
                    throw new NotSupportedException(setting.Key);
            }
        }

        if (model.SiteKeywords == null)
        {
            // 初始化，方便后续操作
            model.SiteKeywords = [];
        }

        if (model.FooterLinks == null || model.FooterLinks.Count == 0)
        {
            // 插入一条空白的，以便界面默认显示一行输入框
            model.FooterLinks = [new TextUrlModel()];
        }
    }

    async Task OnSiteLogoUploadedAsync(FluentInputFileEventArgs file)
    {
        isSiteLogoUploading = true;

        using var memoryStream = new MemoryStream();
        await file.Stream!.CopyToAsync(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);

        var multipartBody = new Microsoft.Kiota.Abstractions.MultipartBody();
        multipartBody.AddOrReplacePart<Stream>("File", MediaTypeNames.Application.Octet, memoryStream, file.Name);
        multipartBody.AddOrReplacePart("Type", MediaTypeNames.Text.Plain, FileType.SiteLogo.ToString());
        multipartBody.AddOrReplacePart("KeepFileName", MediaTypeNames.Text.Plain, Boolean.TrueString);

        var response = await WebApiClient.Api.Files.PostAsync(multipartBody);
        model.SiteLogoUrl = response.Url;
    }

    async Task OnFaviconUploadedAsync(FluentInputFileEventArgs file)
    {
        isFaviconUploading = true;

        using var memoryStream = new MemoryStream();
        await file.Stream!.CopyToAsync(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);

        var multipartBody = new Microsoft.Kiota.Abstractions.MultipartBody();
        multipartBody.AddOrReplacePart<Stream>("File", MediaTypeNames.Application.Octet, memoryStream, file.Name);
        multipartBody.AddOrReplacePart("Type", MediaTypeNames.Text.Plain, FileType.Favicon.ToString());
        multipartBody.AddOrReplacePart("KeepFileName", MediaTypeNames.Text.Plain, Boolean.TrueString);

        var response = await WebApiClient.Api.Files.PostAsync(multipartBody);
        model.FaviconUrl = response.Url;
    }

    private async Task HandleValidSubmit()
    {
        isSaving = true;

        try
        {
            await WebApiClient.Api.Settings.Batch.PutAsync(new Bammemo.Web.Client.WebApis.Client.Models.BatchUpdateSettingByKeyRequest
            {
                Settings = new WebApis.Client.Models.BatchUpdateSettingByKeyRequest_settings
                {
                    AdditionalData = settingKeys.ToDictionary(key => key, key => key switch
                    {
                        SettingKeys.SiteName => NormalizeSettingValue(model.SiteName),
                        SettingKeys.SiteDescription => NormalizeSettingValue(model.SiteDescription),
                        SettingKeys.SiteLogoUrl => NormalizeSettingValue(model.SiteLogoUrl),
                        SettingKeys.FaviconUrl => NormalizeSettingValue(model.FaviconUrl),
                        SettingKeys.SiteKeywords => model.SiteKeywords != null ? JsonSerializer.Serialize(model.SiteKeywords, JsonSourceGenerationContext.Default.ListString) : (object?)null,
                        SettingKeys.FooterLinks => model.FooterLinks != null ? JsonSerializer.Serialize(model.FooterLinks.Where(i => !i.IsEmpty), JsonSourceGenerationContext.Default.IEnumerableTextUrlModel) : (object?)null,
                        _ => throw new NotSupportedException(key)
                    })
                }
            });

            ToastService.ShowSuccess("站点设置保存成功，刷新页面后生效。");
        }
        catch (Exception ex)
        {
            ToastService.ShowError($"保存失败：{ex.Message}");
            Console.WriteLine($"保存 site setting 失败：{ex}");
        }
        finally
        {
            isSaving = false;
        }
    }

    private void HandleAddKeywords()
    {
        if (!String.IsNullOrWhiteSpace(inputedKeyword))
        {
            model.SiteKeywords.Add(inputedKeyword);
        }

        inputedKeyword = null;
    }

    private void HandleRemoveSiteKeyword(int index)
    {
        if (model.SiteKeywords.Count > 1)
        {
            model.SiteKeywords.RemoveAt(index);
        }
        else
        {
            model.SiteKeywords.Clear();
        }

        StateHasChanged();
    }

    private void HandleAddFooterLinkLine()
    {
        model.FooterLinks.Add(new TextUrlModel());
    }

    private void HandleRemoveFooterLinkLine(int index)
    {
        model.FooterLinks.RemoveAt(index);
    }

    private static string? NormalizeSettingValue(string? value)
        => String.IsNullOrWhiteSpace(value) ? null : value;

    public class SiteSettingModel
    {
        [Required]
        public string? SiteName { get; set; }

        public string? SiteDescription { get; set; }

        public List<string>? SiteKeywords { get; set; }

        public List<TextUrlModel>? FooterLinks { get; set; }

        public string? FaviconUrl { get; set; }

        public string? SiteLogoUrl { get; set; }
    }

    public class TextUrlModel
    {
        [Required]
        public string? Text { get; set; }

        public string? Url { get; set; }

        [JsonIgnore]
        public bool IsEmpty => String.IsNullOrWhiteSpace(Text) && String.IsNullOrWhiteSpace(Url);
    }
}
