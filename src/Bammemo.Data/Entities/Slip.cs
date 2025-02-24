using Bammemo.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Bammemo.Data.Entities;

[EntityTypeConfiguration(typeof(SlipConfiguration))]
public class Slip
{
    public int Id { get; set; }
    public required string Content { get; set; }
    public long CreatedAt { get; set; }
    public long? UpdateAt { get; set; }
    public string? FriendlyLinkName { get; set; }
    public int Status { get; set; }
    public string? Title { get; set; }
    public string? Excerpt { get; set; }

    public virtual ICollection<SlipTag>? Tags { get; set; }
}
