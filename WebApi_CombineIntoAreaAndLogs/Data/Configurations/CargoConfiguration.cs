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
    internal class CargoConfiguration : IEntityTypeConfiguration<CargoDal>
    {
        public void Configure(EntityTypeBuilder<CargoDal> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasOne(x => x.Picket)
                .WithMany(p => p.Cargoes);
        }
    }
}
