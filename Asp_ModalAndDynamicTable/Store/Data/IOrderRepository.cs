using Store.Services.Models;

namespace Store.Data
{
    public interface IOrderRepository
    {
        Task Add(IEnumerable<ArticleInput> inputArticles);
        Task<Order?> Get(int id);
        Task<IEnumerable<Order>> GetAll();
        Task Update(Order order);
        Task<bool> IsExist(int id);
    }
}