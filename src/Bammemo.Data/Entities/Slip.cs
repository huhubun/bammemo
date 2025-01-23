using Bammemo.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Bammemo.Data.Entities;

[EntityTypeConfiguration(typeof(SlipConfiguration))]
public class Slip
{
    public required uint Id { get; set; }
    public required string Content { get; set; }
    public long CreatedAt { get; set; }
    public long? UpdateAt { get; set; }
    public string? FriendlyUrl { get; set; }
    public int Status { get; set; }

    public ICollection<SlipTag>? Tags { get; set; }
}
