using AutoMapper;
using Bammemo.Data.Entities;
using Bammemo.Service.Abstractions.Dtos.Settings;

namespace Bammemo.Service.MapperProfiles;

public class SettingProfile : Profile
{
    public SettingProfile()
    {
        CreateMap<Setting, GetSettingByKeyDto>();

        CreateMap<Setting, BatchGetSettingByKeyDto.SettingItemModel>();
    }
}
