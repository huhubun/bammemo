using Bammemo.Web.Client.WebApis.Client;
using Bammemo.Web.Client.WebApis.Client.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.FluentUI.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;
using System.Net;

namespace Bammemo.Web.Client.BammemoComponents.Slips;

public partial class BammemoSlipPropertyEditDialog(
    WebApiClient WebApiClient,
    IToastService ToastService) : IDialogContentComponent<BammemoSlipPropertyEditDialog.SlipPropertyModel>
{
    private EditContext _editContext = default!;
    private bool isLoading = false;
    private bool isFriendlyLinkNameExists = false;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    [Parameter]
    public SlipPropertyModel Content { get; set; } = default!;

    protected override void OnInitialized()
    {
        _editContext = new EditContext(Content);
    }

    private async Task SaveAsync()
    {
        isFriendlyLinkNameExists = false;

        if (_editContext.Validate())
        {
            isLoading = true;

            try
            {
                await WebApiClient.Api.Slips[Content.Id].Property.PutAsync(Content.MapTo<UpdateSlipPropertyRequest>());

                await Dialog.CloseAsync(Content);
            }
            catch (Exception ex)
            {
                if (ex is ProblemDetails problemDetails && problemDetails.Status == (int)HttpStatusCode.Conflict)
                {
                    isFriendlyLinkNameExists = true;
                }
                else
                {
                ToastService.ShowError($"操作失败：{ex.Message}");
                Console.WriteLine($"更新文章属性失败：{ex}");
            }
            }
            finally
            {
                isLoading = false;
            }
        }
    }

    private async Task CancelAsync()
    {
        await Dialog.CancelAsync();
    }

    public record SlipPropertyModel
    {
        public required string Id { get; set; }

        public string? Title { get; set; }

        public string? Excerpt { get; set; }

        [RegularExpression("^[a-zA-Z0-9_-]+$", ErrorMessage = "可读性链接只能包含大小写英文字母、数字、短杠和下划线")]
        public string? FriendlyLinkName { get; set; }
    }

    public class SiteLinkModelValidator
    {
        public static ValidationResult IsValidUrl(string? value, ValidationContext context)
            => Uri.TryCreate(value, UriKind.Absolute, out _) ? ValidationResult.Success : new ValidationResult("Url 格式错误");
    }
}
