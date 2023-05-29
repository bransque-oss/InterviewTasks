using Store.Services.Models;

namespace Store.Data
{
    public interface IArticleRepository
    {
        Task<IEnumerable<Article>> GetAll();
    }
}