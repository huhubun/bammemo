using AutoMapper;
using Bammemo.Data.Entities;
using Bammemo.Service.Abstractions.Dtos.Slips;

namespace Bammemo.Service.MapperProfiles;

public class SlipTagProfile : Profile
{
    public SlipTagProfile()
    {
        CreateMap<SlipTag, SlipTagDto>();

        CreateMap<SlipTag, string>()
            .ConvertUsing(src => src.Tag);

        CreateMap<SlipTagDto, SlipTag>();
    }
}
