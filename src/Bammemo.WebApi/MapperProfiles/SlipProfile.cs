using AutoMapper;
using Bammemo.Data.Entities;
using Bammemo.Service.Abstractions.WebApiModels.Slips;
using Bammemo.Service.Server.Interfaces;

namespace Bammemo.WebApi.MapperProfiles;

public class SlipProfile : Profile
{
    public SlipProfile()
    //public SlipProfile(IIdService idService)
    {
        //CreateMap<CreateSlipModel, Dtos.SlipDto>()
        //    .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow.Ticks));

        CreateMap<Slip, ListSlipResponse.SlipModel>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
            //.AfterMap(async (src, dest, _) =>
            //{
            //    dest.Id = await idService.EncodeAsync(src.Id);
            //});

        CreateMap<CreateSlipRequest, Slip>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow.Ticks));

        CreateMap<Slip, CreateSlipResponse>()
            .IncludeBase<Slip, ListSlipResponse.SlipModel>();
    }
}
