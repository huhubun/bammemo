using AutoMapper;
using Bammemo.Data.Entities;
using Bammemo.Service.Abstractions.Dtos.Slips;
using Bammemo.Web.WebApiModels.Slips;

namespace Bammemo.Web.MapperProfiles;

public class SlipProfile : Profile
{
    public SlipProfile()
    {
        CreateMap<CreateSlipRequest, Slip>();

        CreateMap<Slip, CreateSlipResponse>()
            .IncludeBase<Slip, ListSlipResponse.SlipModel>();

        CreateMap<Slip, UpdateSlipResponse>()
            .IncludeBase<Slip, ListSlipResponse.SlipModel>();

        CreateMap<Slip, GetSlipByIdResponse>()
            .IncludeBase<Slip, ListSlipResponse.SlipModel>();

        CreateMap<Slip, GetSlipByLinkNameResponse>()
            .IncludeBase<Slip, ListSlipResponse.SlipModel>();

        CreateMap<UpdateSlipRequest, Slip>();

        CreateMap<ListSlipQueryRequest, ListSlipQueryRequestDto>();
    }
}
