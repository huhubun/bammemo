namespace Bammemo.Web.Client.BammemoComponents.Settings;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.FluentUI.AspNetCore.Components;
using System.ComponentModel.DataAnnotations;

public partial class BammemoSettingRedirectRuleEditDialog(
    Bammemo.Web.Client.WebApis.Client.WebApiClient WebApiClient,
    IToastService ToastService) : IDialogContentComponent<BammemoSettingRedirectRuleEditDialog.RedirectRuleModel>
{
    private EditContext _editContext = default!;
    private bool isLoading = false;

    [CascadingParameter]
    public FluentDialog Dialog { get; set; } = default!;

    [Parameter]
    public RedirectRuleModel Content { get; set; } = default!;

    protected override void OnInitialized()
    {
        if (Content.HttpStatus == null)
        {
            Content.HttpStatus = RedirectRuleHelper.GetValidHttpStatus().First().Key.ToString();
        }

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
                    await WebApiClient.Api.RedirectRules[Content.Id.Value].PutAsync(Content.MapTo<Bammemo.Web.Client.WebApis.Client.Models.UpdateRedirectRuleRequest>());
                }
                else
                {
                    await WebApiClient.Api.RedirectRules.PostAsync(Content.MapTo<Bammemo.Web.Client.WebApis.Client.Models.CreateRedirectRuleRequest>());
                }

                await Dialog.CloseAsync(Content);
            }
            catch (Exception ex)
            {
                ToastService.ShowError($"操作失败：{ex.Message}");
                Console.WriteLine($"Redirect rule 操作失败：{ex}");
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

    public class RedirectRuleModel
    {
        public int? Id { get; set; }

        [Required]
        public string? Source { get; set; }

        [Required]
        [CustomValidation(typeof(RedirectRuleModelValidator), nameof(RedirectRuleModelValidator.SourceTargetNotSame))]
        public string? Target { get; set; }

        [Required]
        [CustomValidation(typeof(RedirectRuleModelValidator), nameof(RedirectRuleModelValidator.ValidHttpStatus))]
        public string? HttpStatus { get; set; }
    }

    public class RedirectRuleModelValidator
    {
        public static ValidationResult SourceTargetNotSame(string? value, ValidationContext context)
        {
            var model = context.ObjectInstance as RedirectRuleModel;
            return (model.Source != model.Target) ? ValidationResult.Success : new ValidationResult("Source 与 Target 不能相同");
        }

        public static ValidationResult ValidHttpStatus(string? value)
        {
            return RedirectRuleHelper.GetValidHttpStatus().Select(i => i.Key.ToString()).Contains(value) ? ValidationResult.Success : new ValidationResult("不支持的 HTTP 状态码");
        }
    }
}
