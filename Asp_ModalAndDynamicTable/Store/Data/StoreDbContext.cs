using Microsoft.EntityFrameworkCore;
using Store.Data.Configurations;
using Store.Data.Models;

namespace Store.Data
{
    public class StoreDbContext : DbContext
    {
        public StoreDbContext(DbContextOptions<StoreDbContext> options) : base(options)
        {
        }

        public DbSet<OrderDal> Orders { get; set; }
        public DbSet<ArticleDal> Articles { get; set; }
        public DbSet<OrderArticleDal> OrderArticles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new OrderConfiguration());
            modelBuilder.ApplyConfiguration(new ArticleConfiguration());
            modelBuilder.ApplyConfiguration(new OrderArticleConfiguration());

            SeedData.AddData(modelBuilder);
        }
    }
}
