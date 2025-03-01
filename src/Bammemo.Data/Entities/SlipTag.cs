using Bammemo.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Bammemo.Data.Entities;

[EntityTypeConfiguration(typeof(SlipTagConfiguration))]
public class SlipTag
{
    public int Id { get; set; }
    public int SlipId { get; set; }
    public required string Tag { get; set; }

    public virtual Slip? Slip { get; set; }
}
