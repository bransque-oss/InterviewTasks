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
    internal class PicketAreaConfiguration : IEntityTypeConfiguration<PicketAreaDal>
    {
        public void Configure(EntityTypeBuilder<PicketAreaDal> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.AreaName)
                .HasMaxLength(20);
        }
    }
}
