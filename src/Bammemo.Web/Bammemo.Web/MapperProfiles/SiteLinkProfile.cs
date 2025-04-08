using Bammemo.Data.Entities;
using Bammemo.Service.Abstractions.Attributes;
using Bammemo.Service.Abstractions.Dtos.SiteLinks;
using Bammemo.Web.WebApiModels.SiteLinks;

namespace Bammemo.Web.MapperProfiles;

[Map<SiteLink, ListSiteLinkResponse.SiteLinkModel>]
[Map<CreateSiteLinkRequest, SiteLink>]
[Map<UpdateSiteLinkRequest, SiteLink>]
[Map<SiteLink, CreateSiteLinkResponse>]
[Map<SiteLink, ListSiteLinkDto.SiteLinkModel>]
public static partial class SiteLinkProfile
{
}
