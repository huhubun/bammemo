using Bammemo.Web.Client.WebApis.Client;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.FluentUI.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

namespace Bammemo.Web.Client.BammemoComponents.Settings;

public partial class BammemoSettingSiteLinkEditDialog(
    WebApiClient WebApiClient,
    IToastService ToastService) : IDialogContentComponent<BammemoSettingSiteLinkEditDialog.SiteLinkModel>
{
    private EditContext _editContext = default!;
    private bool isLoading = false;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    [Parameter]
    public SiteLinkModel Content { get; set; } = default!;

    protected override void OnInitialized()
    {
        _editContext = new EditContext(Content);
    }

    private async Task SaveAsync()
    {
        if (_editContext.Validate())
        {
            isLoading = true;

            try
            {
                if (Content.Id.HasValue)
                {
                    await WebApiClient.Api.SiteLinks[Content.Id.Value].PutAsync(Content.MapTo<Bammemo.Web.Client.WebApis.Client.Models.UpdateSiteLinkRequest>());
                }
                else
                {
                    await WebApiClient.Api.SiteLinks.PostAsync(Content.MapTo<Bammemo.Web.Client.WebApis.Client.Models.CreateSiteLinkRequest>());
                }

                await Dialog.CloseAsync(Content);
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"操作失败：{ex.Message}");
                Console.WriteLine($"Site link 操作失败：{ex}");
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

    public record SiteLinkModel
    {
        public int? Id { get; set; }

        [Required]
        public string? Name { get; set; }

        [Required]
        [CustomValidation(typeof(SiteLinkModelValidator), nameof(SiteLinkModelValidator.IsValidUrl))]
        public string? Url { get; set; }
    }

    public class SiteLinkModelValidator
    {
        public static ValidationResult IsValidUrl(string? value, ValidationContext context)
            => Uri.TryCreate(value, UriKind.Absolute, out _) ? ValidationResult.Success : new ValidationResult("Url 格式错误");
    }
}
