using Bammemo.Data.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Bammemo.Data.Entities;

[EntityTypeConfiguration(typeof(RedirectRuleConfiguration))]
public class RedirectRule
{
    public int Id { get; set; }
    public required string Source { get; set; }
    public required string Target { get; set; }
    public int HttpStatus { get; set; }
}
