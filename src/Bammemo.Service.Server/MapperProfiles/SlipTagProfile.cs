using AutoMapper;
using Bammemo.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bammemo.Service.Server.MapperProfiles;

public class SlipTagProfile : Profile
{
    public SlipTagProfile()
    {
        CreateMap<SlipTag, Dtos.SlipTagDto>();

        CreateMap<Dtos.SlipTagDto, SlipTag>();
    }
}
