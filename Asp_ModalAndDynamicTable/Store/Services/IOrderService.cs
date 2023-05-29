using Store.Services.Models;
using Store.ViewModels;

namespace Store.Services
{
    public interface IOrderService
    {
        Task CreateOrder(IEnumerable<ArticleInput> inputArticles);
        Task<IEnumerable<Order>> GetAllOrders();
        Task<Order> GetOrder(int id);
        Task UpdateOrder(int id, IList<AddedArticleVm> addedArticles);
    }
}