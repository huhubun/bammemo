using Bammemo.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Bammemo.Data;

public class BammemoDbContext : DbContext
{
    public BammemoDbContext(DbContextOptions<BammemoDbContext> options)
        : base(options)
    { }

    public DbSet<Slip> Slips { get; set; }
}