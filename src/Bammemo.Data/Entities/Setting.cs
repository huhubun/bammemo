using Bammemo.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Bammemo.Data.Entities;

[EntityTypeConfiguration(typeof(SettingConfiguration))]
public class Setting
{
    public int Id { get; set; }
    public required string Key { get; set; }
    public string? Value { get; set; }
    public long CreatedAt { get; set; }
    public long? UpdateAt { get; set; }
}
