using AutoMapper;
using Bammemo.Data.Entities;
using Bammemo.Service.Abstractions.Dtos.Settings;
using Bammemo.Web.WebApiModels.Settings;

namespace Bammemo.Web.MapperProfiles;

public class SettingProfile : Profile
{
    public SettingProfile()
    {
        CreateMap<Setting, GetSettingByKeyDto>();
        CreateMap<Setting, BatchGetSettingByKeyDto.SettingItemModel>();

        CreateMap<Setting, GetSettingByKeyResponse>();
        CreateMap<Setting, BatchGetSettingByKeyResponse.SettingItemModel>();
    }
}
