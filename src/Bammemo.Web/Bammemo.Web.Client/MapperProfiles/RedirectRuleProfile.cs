using AutoMapper;
using Bammemo.Web.Client.BammemoComponents.Settings;

namespace Bammemo.Web.Client.MapperProfiles;

public class RedirectRuleProfile : Profile
{
    public RedirectRuleProfile()
    {
        CreateMap<BammemoSettingRedirectRuleEditDialog.RedirectRuleModel, Bammemo.Web.Client.WebApis.Client.Models.CreateRedirectRuleRequest>();
        CreateMap<BammemoSettingRedirectRuleEditDialog.RedirectRuleModel, Bammemo.Web.Client.WebApis.Client.Models.UpdateRedirectRuleRequest>();
        CreateMap<Bammemo.Web.Client.WebApis.Client.Models.RedirectRuleModel, BammemoSettingRedirectRuleEditDialog.RedirectRuleModel>();
    }
}
