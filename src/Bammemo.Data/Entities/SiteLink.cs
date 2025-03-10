using Bammemo.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Bammemo.Data.Entities;

[EntityTypeConfiguration(typeof(SiteLinkConfiguration))]
public class SiteLink
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Url{ get; set; }
    public long CreatedAt { get; set; }
    public long? UpdateAt { get; set; }
}
