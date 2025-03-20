using AutoMapper;
using Bammemo.Data.Entities;
using Bammemo.Service.Abstractions.Dtos.Slips;
using Bammemo.Service.Attributes;
using Bammemo.Service.Interfaces;

namespace Bammemo.Service.MapperProfiles;

[NeedIdService]
public class SlipProfile : Profile
{
    public SlipProfile(IIdService idService)
    {
        CreateMap<Slip, ListSlipDto>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .AfterMap(async (src, dest, _) =>
            {
                dest.Id = await idService.EncodeAsync(src.Id);
            });

        CreateMap<Slip, SlipDetailDto>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .AfterMap(async (src, dest, _) =>
            {
                dest.Id = await idService.EncodeAsync(src.Id);
            });
    }
}
