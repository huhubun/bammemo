using Bammemo.Service.Abstractions.Attributes;
using Bammemo.Web.Client.BammemoComponents.Settings;
using Bammemo.Web.Client.WebApis.Client.Models;

namespace Bammemo.Web.Client;

// RedirectRule
[Map<BammemoSettingRedirectRuleEditDialog.RedirectRuleModel, CreateRedirectRuleRequest>]
[Map<BammemoSettingRedirectRuleEditDialog.RedirectRuleModel, UpdateRedirectRuleRequest>]
[Map<RedirectRuleModel, BammemoSettingRedirectRuleEditDialog.RedirectRuleModel>]
// SiteLink
[Map<BammemoSettingSiteLinkEditDialog.SiteLinkModel, CreateSiteLinkRequest>]
[Map<BammemoSettingSiteLinkEditDialog.SiteLinkModel, UpdateSiteLinkRequest>]
[Map<SiteLinkModel, BammemoSettingSiteLinkEditDialog.SiteLinkModel>]

public static partial class MapperProfile
{
    static partial void AfterMap(BammemoSettingRedirectRuleEditDialog.RedirectRuleModel source, CreateRedirectRuleRequest target)
    {
        target.HttpStatus = source.HttpStatus != null ? Convert.ToInt32(source.HttpStatus) : null;
    }

    static partial void AfterMap(BammemoSettingRedirectRuleEditDialog.RedirectRuleModel source, UpdateRedirectRuleRequest target)
    {
        target.HttpStatus = source.HttpStatus != null ? Convert.ToInt32(source.HttpStatus) : null;
    }

    static partial void AfterMap(RedirectRuleModel source, BammemoSettingRedirectRuleEditDialog.RedirectRuleModel target)
    {
        target.HttpStatus = source.HttpStatus?.ToString();
    }
}
