using Bammemo.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bammemo.Data;

public class BammemoDbContext(DbContextOptions<BammemoDbContext> options) : DbContext(options)
{
    public DbSet<Slip> Slips { get; set; }
    public DbSet<SlipTag> SlipTags { get; set; }
    public DbSet<Setting> Settings { get; set; }
    public DbSet<RedirectRule> RedirectRules { get; set; }
}