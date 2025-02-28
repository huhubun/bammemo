using Bammemo.Service.Abstractions.SettingModels;
using Bammemo.Service.Abstractions.WebApiModels.Settings;
using Bammemo.Service.Abstractions.WebApiModels.Slips;
using System.Text.Json.Serialization;

namespace Bammemo.Web.Client;

[JsonSourceGenerationOptions(WriteIndented = true)]
[JsonSerializable(typeof(string))]
[JsonSerializable(typeof(TextUrlSetting))]
[JsonSerializable(typeof(List<TextUrlSetting>))]
[JsonSerializable(typeof(SlipDetailDto))]
[JsonSerializable(typeof(ListSlipDto))]
[JsonSerializable(typeof(ListSlipResponse))]
[JsonSerializable(typeof(GetSlipByIdResponse))]
[JsonSerializable(typeof(CreateSlipResponse))]
[JsonSerializable(typeof(UpdateSlipRequest))]
[JsonSerializable(typeof(GetSlipTimesResponse))]
[JsonSerializable(typeof(GetSlipTagsResponse))]
[JsonSerializable(typeof(GetSettingByKeyResponse))]
[JsonSerializable(typeof(BatchGetSettingByKeyResponse))]
internal partial class SourceGenerationContext : JsonSerializerContext
{
}