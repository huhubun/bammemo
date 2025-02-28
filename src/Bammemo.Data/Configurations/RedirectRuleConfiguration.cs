using Bammemo.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bammemo.Data.Configurations;

public class RedirectRuleConfiguration : IEntityTypeConfiguration<RedirectRule>
{
    public void Configure(EntityTypeBuilder<RedirectRule> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id).ValueGeneratedOnAdd();

        builder.HasIndex(s => s.Source).IsUnique();
    }
}
