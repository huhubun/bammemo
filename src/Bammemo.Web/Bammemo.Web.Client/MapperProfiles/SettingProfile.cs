using AutoMapper;
using Bammemo.Service.Abstractions.Dtos.Settings;
using Bammemo.Service.Abstractions.SettingModels;
using Bammemo.Service.Abstractions.WebApiModels.Settings;

namespace Bammemo.Web.Client.MapperProfiles;

public class SettingProfile : Profile
{
    public SettingProfile()
    {
        CreateMap<GetSettingByKeyResponse, GetSettingByKeyDto>();

        CreateMap<BatchGetSettingByKeyResponse, BatchGetSettingByKeyDto>();
        CreateMap<BatchGetSettingByKeyResponse.SettingItemModel, BatchGetSettingByKeyDto.SettingItemModel>();

    }
}
