using AutoMapper;
using Bammemo.Data.Entities;
using Bammemo.Service.Abstractions.WebApiModels.Slips;

namespace Bammemo.WebApi.MapperProfiles;

public class SlipProfile : Profile
{
    public SlipProfile()
    {
        CreateMap<CreateSlipRequest, Slip>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow.Ticks));

        CreateMap<Slip, CreateSlipResponse>()
            .IncludeBase<Slip, ListSlipResponse.SlipModel>();

        CreateMap<Slip, UpdateSlipResponse>()
            .IncludeBase<Slip, ListSlipResponse.SlipModel>();

        CreateMap<UpdateSlipRequest, Slip>();
    }
}
