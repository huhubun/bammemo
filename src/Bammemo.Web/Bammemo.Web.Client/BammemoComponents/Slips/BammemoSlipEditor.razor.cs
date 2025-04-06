using Bammemo.Service.Abstractions.Dtos.Slips;
using Bammemo.Service.Abstractions.Enums;
using Bammemo.Web.Client.WebApis.Client;
using Bammemo.Web.Client.WebApis.Client.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.FluentUI.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Mime;

namespace Bammemo.Web.Client.BammemoComponents.Slips;

public partial class BammemoSlipEditor(
    IJSRuntime jsRuntime,
    WebApiClient WebApiClient)
{
    private string? content = null;
    private string status = ((int)SlipStatus.Public).ToString();
    private bool isSaveButtonLoading = false;
    private ElementReference textarea;
    private bool isAttachmentUploading = false;
    private List<SlipAttachmentModel> attachments = new();

    [Parameter]
    public bool IsEdit { get; set; }

    [Parameter]
    public ListSlipDto? Data { get; set; }

    [Parameter]
    public EventCallback<ListSlipDto> OnSlipSaved { get; set; }

    [Parameter]
    public EventCallback<bool> OnEditCanceled { get; set; }

    private string RenderedSlipAttachmentUploaderId => Data?.Id ?? "NewSlip";

    protected override void OnParametersSet()
    {
        if (Data != null)
        {
            content = Data.Content;
            status = ((int)Data.Status).ToString();
        }
    }

    protected async override Task OnAfterRenderAsync(bool firstRender)
    {
        if (Data != null)
        {
            await jsRuntime.InvokeVoidAsync("bammemo.util.resizeSlipEditorTextarea", textarea);
        }
    }

    private async Task OnSaveClickAsync()
    {
        ArgumentNullException.ThrowIfNull(content);

        if (IsEdit)
        {
            ArgumentNullException.ThrowIfNull(Data);

            var editedStatus = Enum.Parse<SlipStatus>(status);

            var updatedSlip = await WebApiClient.Api.Slips[Data.Id].PutAsync(new UpdateSlipRequest
            {
                Content = content,
                Status = (int)editedStatus
            });

            // Keep slip id here
            var dto = updatedSlip.MapTo<ListSlipDto>();
            dto.Id = Data.Id;

            await OnSlipSaved.InvokeAsync(dto);
        }
        else
        {
            var createdSlip = await WebApiClient.Api.Slips.PostAsync(new CreateSlipRequest
            {
                Content = content,
                Status = (int)Enum.Parse<SlipStatus>(status),
                Attachments = attachments.MapToList<AttachmentModel>()
            });

            await OnSlipSaved.InvokeAsync(createdSlip.MapTo<ListSlipDto>());
        }

        content = String.Empty;
        attachments.Clear();
    }

    private async Task OnAttachmentUploadedAsync(FluentInputFileEventArgs file)
    {
        isAttachmentUploading = true;

        using var memoryStream = new MemoryStream();
        await file.Stream!.CopyToAsync(memoryStream);
        memoryStream.Seek(0, SeekOrigin.Begin);

        var multipartBody = new Microsoft.Kiota.Abstractions.MultipartBody();
        multipartBody.AddOrReplacePart<Stream>("File", MediaTypeNames.Application.Octet, memoryStream, file.Name);
        multipartBody.AddOrReplacePart("Type", MediaTypeNames.Text.Plain, FileType.SlipAttachment.ToString());
        multipartBody.AddOrReplacePart("KeepFileName", MediaTypeNames.Text.Plain, Boolean.FalseString);

        var response = await WebApiClient.Api.Files.PostAsync(multipartBody);

        if (response == null)
        {
            // TODO
        }
        else
        {
            var attachmentModel = response.MapTo<SlipAttachmentModel>();
            attachmentModel.ShowThumbnail = true;
            attachmentModel.Title = file.Name.IndexOf('.') < 0 ? file.Name : file.Name.Substring(0, file.Name.IndexOf('.'));

            attachments.Add(attachmentModel);
        }
    }

    private async Task HandleInsertAttachmentToContentAsync(SlipAttachmentModel slipAttachmentModel)
    {
        var isImage = FileNameHelper.IsImage(slipAttachmentModel.FileName);
        var insertContent = $"{Environment.NewLine}{(isImage ? "!" : null)}[{slipAttachmentModel.Title}]({slipAttachmentModel.Url}){Environment.NewLine}";

        content = await jsRuntime.InvokeAsync<string>("bammemo.util.insertToSlipEditorTextarea", textarea, insertContent);

        // 插入文中的图片默认不显示缩略图
        if (isImage)
        {
            slipAttachmentModel.ShowThumbnail = false;
        }
    }

    public class SlipAttachmentModel
    {
        public int FileMetadataId { get; set; }
        public required string FileName { get; set; }
        public required string Url { get; set; }
        public bool ShowThumbnail { get; set; }
        public string? Title { get; set; }
    }
}