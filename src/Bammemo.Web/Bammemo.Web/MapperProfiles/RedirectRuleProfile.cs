using Bammemo.Data.Entities;
using Bammemo.Service.Abstractions.Attributes;
using Bammemo.Web.WebApiModels.RedirectRules;

namespace Bammemo.Web.MapperProfiles;

[Map<RedirectRule, ListRedirectRuleResponse.RedirectRuleModel>]
[Map<CreateRedirectRuleRequest, RedirectRule>]
[Map<UpdateRedirectRuleRequest, RedirectRule>]
[Map<RedirectRule, CreateRedirectRuleResponse>]
public static partial class RedirectRuleProfile 
{
}
