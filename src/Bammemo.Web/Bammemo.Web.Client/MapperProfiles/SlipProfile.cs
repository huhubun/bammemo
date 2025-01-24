using AutoMapper;
using Bammemo.Service.Abstractions.WebApiModels.Slips;

namespace Bammemo.Web.Client.MapperProfiles;

public class SlipProfile : Profile
{
    public SlipProfile()
    {
        CreateMap<ListSlipResponse.SlipModel, ListSlipDto>();
    }
}
