using AutoMapper;
using Bammemo.Data.Entities;
using Bammemo.Service.Abstractions.Dtos.SiteLinks;

namespace Bammemo.Service.MapperProfiles;

public class SiteLinkProfile : Profile
{
    public SiteLinkProfile()
    {
        CreateMap<SiteLink, ListSiteLinkDto.SiteLinkModel>();
    }
}
