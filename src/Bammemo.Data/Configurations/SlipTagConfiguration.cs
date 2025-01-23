using Bammemo.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Bammemo.Data.Configurations;

public class SlipTagConfiguration : IEntityTypeConfiguration<SlipTag>
{
    public void Configure(EntityTypeBuilder<SlipTag> builder)
    {
        builder.HasKey(st => st.Id);

        builder.HasIndex(st => new {st.Tag, st.SlipId});
        builder.HasIndex(st => new {st.SlipId, st.Tag});
    }
}
