using AutoMapper;
using Bammemo.Data.Entities;

namespace Bammemo.Service.Server.MapperProfiles;

public class SlipTagProfile : Profile
{
    public SlipTagProfile()
    {
        CreateMap<SlipTag, Dtos.SlipTagDto>();

        CreateMap<SlipTag, string>()
            .ConvertUsing(src => src.Tag);

        CreateMap<Dtos.SlipTagDto, SlipTag>();
    }
}
