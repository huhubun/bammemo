﻿using AutoMapper;
using Bammemo.Data.Entities;
using Bammemo.Web.WebApiModels.SiteLinks;

namespace Bammemo.Web.MapperProfiles;

public class SiteLinkProfile : Profile
{
    public SiteLinkProfile()
    {
        CreateMap<SiteLink, ListSiteLinkResponse.SiteLinkModel>();
        CreateMap<CreateSiteLinkRequest, SiteLink>();
        CreateMap<UpdateSiteLinkRequest, SiteLink>();
        CreateMap<SiteLink, CreateSiteLinkResponse>()
            .IncludeBase<SiteLink, ListSiteLinkResponse.SiteLinkModel>();
    }
}
