using Bammemo.Data.Entities;
using Bammemo.Service.Abstractions.Attributes;
using Bammemo.Service.Abstractions.Dtos.Slips;

namespace Bammemo.Web.MapperProfiles;

[Map<SlipTag, SlipTagDto>]
[Map<SlipTagDto, SlipTag>]
public partial class SlipTagProfile 
{
}
