using AutoMapper;
using Bammemo.Data.Entities;
using Bammemo.Service.Abstractions.Dtos;
using Bammemo.Service.Abstractions.WebApiModels.Slips;
using Bammemo.Service.Server.Interfaces;

namespace Bammemo.Service.Server.MapperProfiles;

public class SlipProfile : Profile
{
    public SlipProfile(IIdService idService)
    {
        CreateMap<Slip, ListSlipResponse.SlipModel>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .AfterMap(async (src, dest, _) =>
            {
                dest.Id = await idService.EncodeAsync(src.Id);
            });

        CreateMap<Slip, ListSlipDto>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .AfterMap(async (src, dest, _) =>
            {
                dest.Id = await idService.EncodeAsync(src.Id);
            });
    }
}
