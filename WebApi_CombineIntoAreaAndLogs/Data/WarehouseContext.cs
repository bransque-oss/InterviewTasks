using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Data.Configurations;
using Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class WarehouseContext  : DbContext
    {
        public WarehouseContext(DbContextOptions<WarehouseContext> options) : base(options)
        {         
        }

        public DbSet<WarehouseDal> Warehouses { get; set; }
        public DbSet<PicketDal> Pickets { get; set; }
        public DbSet<AreaDal> Areas { get; set; }
        public DbSet<CargoDal> Cargoes { get; set; }
        public DbSet<PicketAreaDal> PicketAreaHistory { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new WarehouseConfiguration());
            modelBuilder.ApplyConfiguration(new PicketConfiguration());
            modelBuilder.ApplyConfiguration(new AreaConfiguration());
            modelBuilder.ApplyConfiguration(new CargoConfiguration());
        }
    }
}
