using AutoMapper;
using Bammemo.Service.Abstractions.Dtos.Slips;

namespace Bammemo.Web.Client.MapperProfiles;

public class SlipProfile : Profile
{
    public SlipProfile()
    {
        CreateMap<Bammemo.Web.Client.WebApis.Client.Models.CreateSlipResponse, ListSlipDto>();
        CreateMap<Bammemo.Web.Client.WebApis.Client.Models.UpdateSlipResponse, ListSlipDto>();
        CreateMap<Bammemo.Web.Client.WebApis.Client.Models.GetSlipByLinkNameResponse, SlipDetailDto>();
        CreateMap<Bammemo.Web.Client.WebApis.Client.Models.SlipModel, SlipDetailDto>();
        CreateMap<Bammemo.Web.Client.WebApis.Client.Models.SlipModel, ListSlipDto>();
    }
}
