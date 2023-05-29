using Microsoft.EntityFrameworkCore;
using Store.Services.Models;

namespace Store.Data
{
    public class ArticleRepository : IArticleRepository
    {
        private readonly StoreDbContext _context;

        public ArticleRepository(StoreDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Article>> GetAll()
        {
            return await _context.Articles
                .Select(x => new Article
                {
                    Id = x.Id,
                    Name = x.Name,
                    Price = x.Price,
                }).ToArrayAsync();
        }
    }
}
