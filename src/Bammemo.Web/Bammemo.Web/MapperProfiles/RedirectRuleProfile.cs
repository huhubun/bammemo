using AutoMapper;
using Bammemo.Data.Entities;
using Bammemo.Web.WebApiModels.RedirectRules;

namespace Bammemo.Web.MapperProfiles;

public class RedirectRuleProfile : Profile
{
    public RedirectRuleProfile()
    {
        CreateMap<RedirectRule, ListRedirectRuleResponse.RedirectRuleModel>();
        CreateMap<CreateRedirectRuleRequest, RedirectRule>();
        CreateMap<UpdateRedirectRuleRequest, RedirectRule>();
        CreateMap<RedirectRule, CreateRedirectRuleResponse>()
            .IncludeBase<RedirectRule, ListRedirectRuleResponse.RedirectRuleModel>();
    }
}
