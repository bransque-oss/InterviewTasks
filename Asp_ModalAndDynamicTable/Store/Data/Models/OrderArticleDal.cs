namespace Store.Data.Models
{
    public class OrderArticleDal
    {
        public int OrderId { get; set; }
        public int ArticleId { get; set; }
        public int ArticleQuantity { get; set; }

        public OrderDal Order { get; set; }
        public ArticleDal Article { get; set; }
    }
}
