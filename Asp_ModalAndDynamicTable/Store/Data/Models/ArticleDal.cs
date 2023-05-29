namespace Store.Data.Models
{
    public class ArticleDal
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        public IList<OrderDal> Orders { get; set; }
        public IList<OrderArticleDal> OrderArticles { get; set; }
    }
}
