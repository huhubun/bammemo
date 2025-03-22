using AutoMapper;
using Bammemo.Service.Abstractions.Dtos.SiteLinks;
using Bammemo.Web.Client.BammemoComponents.Settings;

namespace Bammemo.Web.Client.MapperProfiles;

public class SiteLinkProfile : Profile
{
    public SiteLinkProfile()
    {
        CreateMap<BammemoSettingSiteLinkEditDialog.SiteLinkModel, Bammemo.Web.Client.WebApis.Client.Models.CreateSiteLinkRequest>();
        CreateMap<BammemoSettingSiteLinkEditDialog.SiteLinkModel, Bammemo.Web.Client.WebApis.Client.Models.UpdateSiteLinkRequest>();
        CreateMap<Bammemo.Web.Client.WebApis.Client.Models.SiteLinkModel, BammemoSettingSiteLinkEditDialog.SiteLinkModel>();

        CreateMap<Bammemo.Web.Client.WebApis.Client.Models.ListSiteLinkResponse, ListSiteLinkDto>();
        CreateMap<Bammemo.Web.Client.WebApis.Client.Models.SiteLinkModel, ListSiteLinkDto.SiteLinkModel>();
    }
}
