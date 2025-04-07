using Bammemo.Data.Entities;
using Bammemo.Service.Abstractions.Attributes;
using Bammemo.Service.Abstractions.Dtos.Slips;
using Bammemo.Service.Models.Slips;
using Bammemo.Web.WebApiModels.Slips;

namespace Bammemo.Web.MapperProfiles;

[Map<CreateSlipRequest, Slip>]
[Map<Slip, CreateSlipResponse>]
[Map<Slip, UpdateSlipResponse>]
[Map<Slip, GetSlipByIdResponse>]
[Map<Slip, GetSlipByLinkNameResponse>]
[Map<UpdateSlipRequest, Slip>]
[Map<ListSlipQueryRequest, ListSlipQueryRequestDto>]
[Map<Slip, ListSlipDto>]
[Map<Slip, SlipDetailDto>]
[Map<Slip, ListSlipResponse.SlipModel>]
[Map<UpdateSlipPropertyRequest, Slip>(ToExistsOnly = true)]
[Map<CreateSlipRequest.AttachmentModel, AddSlipAttachmentInfo>]
[Map<SlipAttachmentDto, ListSlipResponse.SlipAttachmentModel>]
[Map<UpdateSlipRequest.AttachmentModel, AddSlipAttachmentInfo>]
public static partial class SlipProfile
{
    static partial void AfterMap(Slip source, ListSlipResponse.SlipModel target)
    {
        target.Tags = source.Tags == null ? [] : source.Tags.Select(t => t.Tag).ToArray();
    }
}
