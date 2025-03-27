using Bammemo.Data.Entities;
using Bammemo.Service.Abstractions.Attributes;
using Bammemo.Service.Abstractions.Dtos.Settings;
using Bammemo.Service.Storages;
using Bammemo.Web.WebApiModels.Settings;

namespace Bammemo.Web.MapperProfiles;

[Map<Setting, GetSettingByKeyDto>]
[Map<Setting, BatchGetSettingByKeyDto.SettingItemModel>]
[Map<Setting, GetSettingByKeyResponse>]
[Map<Setting, BatchGetSettingByKeyResponse.SettingItemModel>]
[Map<StorageTypeInfo, GetStorageTypeInfosResponse.StorageTypeInfoModel>]
public partial class SettingProfile
{
}
