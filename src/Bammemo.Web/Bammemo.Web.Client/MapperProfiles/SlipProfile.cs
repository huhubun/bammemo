using Bammemo.Service.Abstractions.Attributes;
using Bammemo.Service.Abstractions.Dtos.Slips;
using Bammemo.Web.Client.WebApis.Client.Models;

namespace Bammemo.Web.Client.MapperProfiles;

[Map<CreateSlipResponse, ListSlipDto>]
[Map<UpdateSlipResponse, ListSlipDto>]
[Map<GetSlipByIdResponse, SlipDetailDto>]
[Map<GetSlipByLinkNameResponse, SlipDetailDto>]
[Map<SlipModel, SlipDetailDto>]
[Map<SlipModel, ListSlipDto>]
public static partial class SlipProfile
{
}
