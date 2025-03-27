using Bammemo.Service.Abstractions.Attributes;
using Bammemo.Service.Abstractions.Dtos.SiteLinks;
using Bammemo.Web.Client.BammemoComponents.Settings;
using Bammemo.Web.Client.WebApis.Client.Models;

namespace Bammemo.Web.Client.MapperProfiles;

[Map<BammemoSettingSiteLinkEditDialog.SiteLinkModel, CreateSiteLinkRequest>]
[Map<BammemoSettingSiteLinkEditDialog.SiteLinkModel, UpdateSiteLinkRequest>]
[Map<SiteLinkModel, BammemoSettingSiteLinkEditDialog.SiteLinkModel>]
[Map<ListSiteLinkResponse, ListSiteLinkDto>]
[Map<SiteLinkModel, ListSiteLinkDto.SiteLinkModel>]
public static partial class SiteLinkProfile
{
}
