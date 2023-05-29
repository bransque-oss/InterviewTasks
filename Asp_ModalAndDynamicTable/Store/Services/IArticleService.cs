using Store.Services.Models;

namespace Store.Services
{
    public interface IArticleService
    {
        Task<IEnumerable<Article>> GetArticles();
    }
}