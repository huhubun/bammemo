using AutoMapper;
using Bammemo.Service.Abstractions.WebApiModels.RedirectRules;
using Bammemo.Web.Client.BammemoComponents.Settings;

namespace Bammemo.Web.Client.MapperProfiles;

public class RedirectRuleProfile : Profile
{
    public RedirectRuleProfile()
    {
        CreateMap<BammemoSettingRedirectRuleEditDialog.RedirectRuleModel, CreateRedirectRuleRequest>();
        CreateMap<BammemoSettingRedirectRuleEditDialog.RedirectRuleModel, UpdateRedirectRuleRequest>();
        CreateMap<ListRedirectRuleResponse.RedirectRuleModel, BammemoSettingRedirectRuleEditDialog.RedirectRuleModel>();
    }
}
