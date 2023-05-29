using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Data.Configurations
{
    internal class WarehouseConfiguration : IEntityTypeConfiguration<WarehouseDal>
    {
        public void Configure(EntityTypeBuilder<WarehouseDal> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(x => x.Deleted).HasDefaultValue(false);
        }
    }
}
