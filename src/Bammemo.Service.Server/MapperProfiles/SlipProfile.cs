using AutoMapper;
using Bammemo.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bammemo.Service.Server.MapperProfiles;

public class SlipProfile : Profile
{
    public SlipProfile()
    {
        CreateMap<Slip, Dtos.SlipDto>();

        CreateMap<Dtos.SlipDto, Slip>();

    }
}
