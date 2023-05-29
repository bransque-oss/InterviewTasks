using Store.Data;
using Store.Services.Models;

namespace Store.Services
{
    public class ArticleService : IArticleService
    {
        private readonly IArticleRepository _repo;
        private readonly ILogger<ArticleService> _logger;

        public ArticleService(IArticleRepository repo, ILogger<ArticleService> logger)
        {
            _logger = logger;
            _repo = repo;
        }

        public async Task<IEnumerable<Article>> GetArticles()
        {
            try
            {
                return await _repo.GetAll();
            }
            catch (Exception ex)
            {
                var message = "Unable to get articles list.";
                _logger.LogError(message, ex);
                throw new Exception(message);
            }
        }
    }
}
