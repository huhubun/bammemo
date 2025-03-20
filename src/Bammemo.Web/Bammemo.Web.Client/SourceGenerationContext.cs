using Bammemo.Service.Abstractions.Dtos.Analytics;
using Bammemo.Service.Abstractions.Dtos.Settings;
using Bammemo.Service.Abstractions.Dtos.SiteLinks;
using Bammemo.Service.Abstractions.SettingModels;
using Bammemo.Service.Abstractions.WebApiModels.RedirectRules;
using Bammemo.Service.Abstractions.WebApiModels.Settings;
using Bammemo.Service.Abstractions.WebApiModels.SiteLinks;
using Bammemo.Service.Abstractions.WebApiModels.Slips;
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
[JsonSerializable(typeof(ListSlipResponse))]
[JsonSerializable(typeof(ListSlipResponse.SlipModel))]
[JsonSerializable(typeof(ListSlipResponse.SlipModel[]))]
[JsonSerializable(typeof(GetSlipByIdResponse))]
[JsonSerializable(typeof(CreateSlipRequest))]
[JsonSerializable(typeof(CreateSlipResponse))]
[JsonSerializable(typeof(UpdateSlipRequest))]
[JsonSerializable(typeof(GetSlipTimesRequest))]
[JsonSerializable(typeof(GetSlipTimesResponse))]
[JsonSerializable(typeof(GetSlipTagsResponse))]
[JsonSerializable(typeof(GetSettingByKeyResponse))]
[JsonSerializable(typeof(BatchGetSettingByKeyResponse))]
[JsonSerializable(typeof(BatchGetSettingByKeyResponse.SettingItemModel))]
[JsonSerializable(typeof(BatchGetSettingByKeyResponse.SettingItemModel[]))]
[JsonSerializable(typeof(ListRedirectRuleResponse))]
[JsonSerializable(typeof(ListRedirectRuleResponse.RedirectRuleModel))]
[JsonSerializable(typeof(ListRedirectRuleResponse.RedirectRuleModel[]))]
[JsonSerializable(typeof(CreateRedirectRuleResponse))]
[JsonSerializable(typeof(UpdateRedirectRuleRequest))]
[JsonSerializable(typeof(UpdateSettingByKeyRequest))]
[JsonSerializable(typeof(BatchUpdateSettingByKeyRequest))]
[JsonSerializable(typeof(ListSiteLinkResponse))]
[JsonSerializable(typeof(ListSiteLinkResponse.SiteLinkModel))]
[JsonSerializable(typeof(ListSiteLinkResponse.SiteLinkModel[]))]
[JsonSerializable(typeof(CreateSiteLinkRequest))]
[JsonSerializable(typeof(CreateSiteLinkResponse))]
[JsonSerializable(typeof(UpdateSiteLinkRequest))]
[JsonSerializable(typeof(UpdateSlipResponse))]
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
[JsonSerializable(typeof(CreateRedirectRuleRequest))]
[JsonSerializable(typeof(BatchGetSettingByKeyRequest))]
[JsonSerializable(typeof(GetSlipByIdRequest))]
[JsonSerializable(typeof(ListSlipQueryRequest))]
[JsonSerializable(typeof(TencentCloudSettingModel))]
internal partial class SourceGenerationContext : JsonSerializerContext
{
}