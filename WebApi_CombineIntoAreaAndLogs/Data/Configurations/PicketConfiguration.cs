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
    internal class PicketConfiguration : IEntityTypeConfiguration<PicketDal>
    {
        public void Configure(EntityTypeBuilder<PicketDal> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Number).IsRequired();
            builder.Property(x => x.Deleted).HasDefaultValue(false);
            builder.HasOne(p => p.Warehouse)
                .WithMany(w => w.Pickets)
                .HasForeignKey(p => p.WarehouseId);
            builder.HasOne(p => p.Area)
                .WithMany(a => a.Pickets)
                .HasForeignKey(p => p.AreaId);
        }
    }
}
