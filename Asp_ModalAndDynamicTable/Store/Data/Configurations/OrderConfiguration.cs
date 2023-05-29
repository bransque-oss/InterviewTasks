using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Data.Models;

namespace Store.Data.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<OrderDal>
    {
        public void Configure(EntityTypeBuilder<OrderDal> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasMany(o => o.Articles)
                .WithMany(a => a.Orders)
                .UsingEntity<OrderArticleDal>(
                    oa => oa.HasOne<ArticleDal>(x => x.Article).WithMany(a => a.OrderArticles).HasForeignKey(x => x.ArticleId),
                    oa => oa.HasOne<OrderDal>(x => x.Order).WithMany(o => o.OrderArticles).HasForeignKey(x => x.OrderId));
        }
    }
}
