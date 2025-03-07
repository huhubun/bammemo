using Bammemo.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bammemo.Data.Configurations;

public class SiteLinkConfiguration : IEntityTypeConfiguration<SiteLink>
{
    public void Configure(EntityTypeBuilder<SiteLink> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id).ValueGeneratedOnAdd();
    }
}
