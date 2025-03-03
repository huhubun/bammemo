using AutoMapper;
using Bammemo.Data.Entities;
using Bammemo.Service.Abstractions.Dtos;
using Bammemo.Service.Abstractions.WebApiModels.Slips;
using Bammemo.Service.Attributes;
using Bammemo.Service.Interfaces;

namespace Bammemo.Service.MapperProfiles;

[NeedIdService]
public class SlipProfile : Profile
{
    public SlipProfile(IIdService idService)
    {
        CreateMap<Slip, ListSlipResponse.SlipModel>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags == null ? Enumerable.Empty<string>() : src.Tags.Select(t => t.Tag)))
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

        CreateMap<Slip, SlipDetailDto>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .AfterMap(async (src, dest, _) =>
            {
                dest.Id = await idService.EncodeAsync(src.Id);
            });
    }
}
