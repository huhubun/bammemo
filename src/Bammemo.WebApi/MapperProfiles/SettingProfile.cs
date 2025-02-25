using AutoMapper;
using Bammemo.Data.Entities;
using Bammemo.Service.Abstractions.WebApiModels.Settings;

namespace Bammemo.WebApi.MapperProfiles;

public class SettingProfile : Profile
{
    public SettingProfile()
    {
        CreateMap<Setting, GetSettingByKeyResponse>();
        CreateMap<Setting, BatchGetSettingByKeyResponse.SettingItemModel>();
    }
}
