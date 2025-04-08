using Bammemo.Service.Abstractions.Attributes;
using Bammemo.Service.Abstractions.Dtos.Slips;
using Bammemo.Web.Client.BammemoComponents.Slips;
using Bammemo.Web.Client.WebApis.Client.Models;

namespace Bammemo.Web.Client.MapperProfiles;

[Map<CreateSlipResponse, ListSlipDto>]
[Map<UpdateSlipResponse, ListSlipDto>]
[Map<GetSlipByIdResponse, SlipDetailDto>]
[Map<GetSlipByLinkNameResponse, SlipDetailDto>]
[Map<SlipModel, SlipDetailDto>]
[Map<SlipModel, ListSlipDto>]
[Map<ListSlipDto, BammemoSlipPropertyEditDialog.SlipPropertyModel>]
[Map<BammemoSlipPropertyEditDialog.SlipPropertyModel, ListSlipDto>(ToExistsOnly = true)]
[Map<BammemoSlipPropertyEditDialog.SlipPropertyModel, UpdateSlipPropertyRequest>]
[Map<UploadFileResponse, BammemoSlipEditor.SlipAttachmentModel>]
[Map<BammemoSlipEditor.SlipAttachmentModel, AttachmentModel>]
[Map<SlipAttachmentModel, SlipAttachmentDto>]
[Map<SlipAttachmentDto, BammemoSlipEditor.SlipAttachmentModel>]
[Map<BammemoSlipEditor.SlipAttachmentModel, SlipAttachmentDto>]
[Map<SlipAttachmentModel, AttachmentModel>]
public static partial class SlipProfile
{
}
