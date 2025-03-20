using AutoMapper;
using Bammemo.Service.Abstractions.Dtos.Settings;
using Bammemo.Service.Abstractions.SettingModels;
using Bammemo.Service.Abstractions.WebApiModels.Settings;
using Bammemo.Web.Client.Extensions.SettingModels;
using Bammemo.Web.Client.Models.Settings;

namespace Bammemo.Web.Client.MapperProfiles;

public class SettingProfile : Profile
{
    public SettingProfile()
    {
        CreateMap<GetSettingByKeyResponse, GetSettingByKeyDto>();

        CreateMap<BatchGetSettingByKeyResponse, BatchGetSettingByKeyDto>();
        CreateMap<BatchGetSettingByKeyResponse.SettingItemModel, BatchGetSettingByKeyDto.SettingItemModel>();

        CreateMap<TencentCloudSetting, TencentCloudSettingModel>()
            .AfterMap((src, dest) =>
            {
                if (dest.Cos.IsNullOrWhiteSpace())
                {
                    dest.Cos = null;
                    dest.EnableCos = false;
                }
                else
                {
                    dest.EnableCos = true;
                }
            });

        CreateMap<TencentCloudSettingModel, TencentCloudSetting>()
            .AfterMap((src, dest) =>
            {
                if (!src.EnableCos)
                {
                    dest.Cos = null;
                }
            });

    }
}
