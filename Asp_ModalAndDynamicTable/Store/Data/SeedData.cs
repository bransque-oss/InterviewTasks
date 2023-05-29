using Microsoft.EntityFrameworkCore;
using Store.Data.Models;

namespace Store.Data
{
    public static class SeedData
    {
        public static void AddData(ModelBuilder builder)
        {
            builder.Entity<OrderDal>().HasData(new OrderDal[]
            {
                new OrderDal { Id = 1 },
                new OrderDal { Id = 2 }
            });

            builder.Entity<ArticleDal>().HasData(new ArticleDal[]
            {
                new ArticleDal { Id = 1, Name = "Труба", Price = 50.5M },
                new ArticleDal { Id = 2, Name = "Корпус", Price = 199 },
                new ArticleDal { Id = 3, Name = "Набор креплений", Price = 23.78M },
                new ArticleDal { Id = 4, Name = "Утеплитель", Price = 10.11M },
            });

            builder.Entity<OrderArticleDal>().HasData(new OrderArticleDal[]
            {
                new OrderArticleDal { OrderId = 1, ArticleId = 1, ArticleQuantity = 4 },
                new OrderArticleDal { OrderId = 1, ArticleId = 2, ArticleQuantity = 1 },
                new OrderArticleDal { OrderId = 1, ArticleId = 4, ArticleQuantity = 2 },
                new OrderArticleDal { OrderId = 2, ArticleId = 3, ArticleQuantity = 3 }
            });
        }
    }
}
