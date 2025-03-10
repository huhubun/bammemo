using Bammemo.Service.Abstractions.WebApiModels.Settings;
using FluentValidation;

namespace Bammemo.Web.Validators.WebApis;

public class BatchUpdateSettingByKeyRequestValidator : AbstractValidator<BatchUpdateSettingByKeyRequest>
{
    public BatchUpdateSettingByKeyRequestValidator()
    {
        RuleFor(r => r.Settings).NotNull();
        RuleFor(r => r.Settings.Keys).Must(SettingKeys.VerifyKeys).WithMessage((_, keys) =>
        {
            SettingKeys.TryVerifyKeys(keys, out var wrongKeys);

            return $"Invalid setting keys: {String.Join(", ", wrongKeys)}";
        });
    }
}
