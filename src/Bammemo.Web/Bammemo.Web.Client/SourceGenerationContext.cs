using Bammemo.Service.Abstractions.Dtos.Analytics;
using Bammemo.Service.Abstractions.Dtos.Settings;
using Bammemo.Service.Abstractions.Dtos.SiteLinks;
using Bammemo.Service.Abstractions.Dtos.Slips;
using Bammemo.Service.Abstractions.SettingModels;
using Bammemo.Web.Client.Models.Settings;
using System.Text.Json.Serialization;

namespace Bammemo.Web.Client;

[JsonSourceGenerationOptions(PropertyNamingPolicy = JsonKnownNamingPolicy.CamelCase)]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(string[]))]
[JsonSerializable(typeof(Dictionary<string, string?>))]
[JsonSerializable(typeof(KeyValuePair<string, string>[]))]
[JsonSerializable(typeof(long))]
[JsonSerializable(typeof(long[]))]
[JsonSerializable(typeof(TextUrlSetting))]
[JsonSerializable(typeof(List<TextUrlSetting>))]
[JsonSerializable(typeof(ListSiteLinkDto))]
[JsonSerializable(typeof(ListSiteLinkDto.SiteLinkModel))]
[JsonSerializable(typeof(ListSiteLinkDto.SiteLinkModel[]))]
[JsonSerializable(typeof(GetSlipTagsDto))]
[JsonSerializable(typeof(GetSlipTimesDto))]
[JsonSerializable(typeof(BatchGetSettingByKeyDto))]
[JsonSerializable(typeof(BatchGetSettingByKeyDto.SettingItemModel))]
[JsonSerializable(typeof(BatchGetSettingByKeyDto.SettingItemModel[]))]
[JsonSerializable(typeof(GetSettingByKeyDto))]
[JsonSerializable(typeof(ListSlipDto))]
[JsonSerializable(typeof(SlipDetailDto))]
[JsonSerializable(typeof(SlipTagDto))]
[JsonSerializable(typeof(TencentCloudSettingModel))]
internal partial class SourceGenerationContext : JsonSerializerContext
{
}