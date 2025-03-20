using Bammemo.Web.Client.Extensions.SettingModels;
using System.ComponentModel.DataAnnotations;

namespace Bammemo.Web.Client.Models.Settings;

public class TencentCloudSettingModel
{
    [Required]
    public string? AppId { get; set; }
    [Required]
    public string? SecretId { get; set; }
    [Required]
    public string? SecretKey { get; set; }

    [CustomValidation(typeof(TencentCloudSettingModelValidator), nameof(TencentCloudSettingModelValidator.CosValidator))]
    public bool EnableCos { get; set; }

    public CosSettingModel? Cos { get; set; }

    public class CosSettingModel
    {
        public string? Region { get; set; }
        public string? Bucket { get; set; }
    }

    public class TencentCloudSettingModelValidator
    {
        public static ValidationResult? CosValidator(bool enableCos, ValidationContext context)
        {
            var cosSetting = ((TencentCloudSettingModel)context.ObjectInstance).Cos;

            if (enableCos && cosSetting.IsNullOrWhiteSpace())
            {
                return new ValidationResult("COS 未配置");
            }

            return ValidationResult.Success;
        }
    }
}