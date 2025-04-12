using Bammemo.Service.Abstractions.Attributes;
using Bammemo.Service.Abstractions.Dtos.Analytics;
using Bammemo.Web.Client.WebApis.Client.Models;

namespace Bammemo.Web.Client.MapperProfiles;

[Map<GetSlipTagsResponse, GetSlipTagsDto>]
[Map<TagItemAnalyticModel, GetSlipTagsDto.TagItemAnalyticModel>]
public static partial class AnalyticProfile
{
}
