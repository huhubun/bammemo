using Bammemo.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bammemo.Data.Configurations;

public class SlipConfiguration : IEntityTypeConfiguration<Slip>
{
    public void Configure(EntityTypeBuilder<Slip> builder)
    {
        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id).ValueGeneratedOnAdd();

        builder.HasIndex(s => s.CreatedAt);
    }
}
