namespace Store.Data.Models
{
    public class OrderDal
    {
        public int Id { get; set; }

        public IList<ArticleDal> Articles { get; set; }
        public IList<OrderArticleDal> OrderArticles { get; set; }
    }
}
