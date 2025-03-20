using AutoMapper;
using Bammemo.Data.Entities;
using Bammemo.Service.Attributes;
using Bammemo.Service.Interfaces;
using Bammemo.Web.WebApiModels.Slips;

namespace Bammemo.Web.MapperProfiles;

[NeedIdService]
public class SlipIdProfile : Profile
{
    public SlipIdProfile(IIdService idService)
    {
        CreateMap<Slip, ListSlipResponse.SlipModel>()
            .ForMember(dest => dest.Id, opt => opt.Ignore())
            .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags == null ? Enumerable.Empty<string>() : src.Tags.Select(t => t.Tag)))
            .AfterMap(async (src, dest, _) =>
            {
                dest.Id = await idService.EncodeAsync(src.Id);
            });
    }
}
