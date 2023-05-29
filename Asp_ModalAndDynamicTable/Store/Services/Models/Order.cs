namespace Store.Services.Models
{
    public class Order
    {
        public int Id { get; set; }
        public IEnumerable<OrderArticle> Articles { get; set; }
    }
}
