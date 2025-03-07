using AutoMapper;
using Bammemo.Service.Abstractions.WebApiModels.SiteLinks;
using Bammemo.Web.Client.BammemoComponents.Settings;

namespace Bammemo.Web.Client.MapperProfiles;

public class SiteLinkProfile : Profile
{
    public SiteLinkProfile()
    {
        CreateMap<BammemoSettingSiteLinkEditDialog.SiteLinkModel, CreateSiteLinkRequest>();
        CreateMap<BammemoSettingSiteLinkEditDialog.SiteLinkModel, UpdateSiteLinkRequest>();
        CreateMap<ListSiteLinkResponse.SiteLinkModel, BammemoSettingSiteLinkEditDialog.SiteLinkModel>();
    }
}
