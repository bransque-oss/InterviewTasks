using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Data.Models;

namespace Store.Data.Configurations
{
    public class OrderArticleConfiguration : IEntityTypeConfiguration<OrderArticleDal>
    {
        public void Configure(EntityTypeBuilder<OrderArticleDal> builder)
        {
            builder.ToTable(t => t.HasCheckConstraint("CK_Articles_Price", $"{nameof(OrderArticleDal.ArticleQuantity)} > 0"));
        }
    }
}
