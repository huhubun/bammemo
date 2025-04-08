using Bammemo.Service.Abstractions.Dtos.Slips;
using Bammemo.Web.Client.WebApis.Client;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.FluentUI.AspNetCore.Components;

namespace Bammemo.Web.Client.BammemoComponents.Slips;

public partial class BammemoSlipImageAttachmentPreviewDialog(
    WebApiClient WebApiClient,
    IToastService ToastService) : IDialogContentComponent<BammemoSlipImageAttachmentPreviewDialog.ImageAttachmentPreviewModel>
{
    private EditContext _editContext = default!;
    private bool isLoading = false;
    private bool isFriendlyLinkNameExists = false;
    private GridItemSize? _gridItemSize = null;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    [Parameter]
    public ImageAttachmentPreviewModel Content { get; set; } = default!;

    public string? ImageUrl => Content.Attachments[Content.CurrentIndex].Url;
    public string? ViewUrl
    {
        get
        {
            if (ImageUrl == null)
            {
                return null;
            }

            return UrlHelper.AppendQueryString(ImageUrl, KeyValuePair.Create("response-content-disposition", "inline")).ToString();
        }
    }
    public string? DownloadUrl
    {
        get
        {
            if (ImageUrl == null)
            {
                return null;
            }

            return UrlHelper.AppendQueryString(ImageUrl, KeyValuePair.Create("response-content-disposition", "attachment")).ToString();
        }
    }

    protected override void OnInitialized()
    {
        _editContext = new EditContext(Content);
    }

    public record ImageAttachmentPreviewModel
    {
        public int CurrentIndex { get; set; }
        public required List<SlipAttachmentDto> Attachments { get; set; }
    }
}
