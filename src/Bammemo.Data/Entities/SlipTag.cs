using Bammemo.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Bammemo.Data.Entities;

[EntityTypeConfiguration(typeof(SlipTagConfiguration))]
public class SlipTag
{
    public required uint Id { get; set; }
    public required uint SlipId { get; set; }
    public required string Tag { get; set; }
}
