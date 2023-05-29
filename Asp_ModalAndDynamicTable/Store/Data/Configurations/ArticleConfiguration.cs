using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Store.Data.Models;

namespace Store.Data.Configurations
{
    public class ArticleConfiguration : IEntityTypeConfiguration<ArticleDal>
    {
        public void Configure(EntityTypeBuilder<ArticleDal> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Price).HasColumnType("decimal(8, 2)");
            builder.ToTable(t => t.HasCheckConstraint("CK_Articles_Price", $"{nameof(ArticleDal.Price)} > 0"));

            builder.Property(x => x.Name).HasMaxLength(50);
        }
    }
}
