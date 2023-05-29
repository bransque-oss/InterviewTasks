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
    internal class AreaConfiguration : IEntityTypeConfiguration<AreaDal>
    {
        public void Configure(EntityTypeBuilder<AreaDal> builder)
        {
            builder.HasKey(x => x.Id);
        }
    }
}
