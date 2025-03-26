using AutoMapper;
using Bammemo.Service.Abstractions.Dtos.SiteLinks;

namespace Bammemo.Web.Client.MapperProfiles;

public class SiteLinkProfile : Profile
{
    public SiteLinkProfile()
    {
        CreateMap<Bammemo.Web.Client.WebApis.Client.Models.ListSiteLinkResponse, ListSiteLinkDto>();
        CreateMap<Bammemo.Web.Client.WebApis.Client.Models.SiteLinkModel, ListSiteLinkDto.SiteLinkModel>();
    }
}
