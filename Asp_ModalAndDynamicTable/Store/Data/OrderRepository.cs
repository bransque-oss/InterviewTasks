using Microsoft.EntityFrameworkCore;
using Store.Data.Models;
using Store.Services.Models;

namespace Store.Data
{
    public class OrderRepository : IOrderRepository
    {
        private readonly StoreDbContext _context;

        public OrderRepository(StoreDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetAll()
        {
            var orders = _context.Orders
                .Include(o => o.OrderArticles)
                .ThenInclude(oa => oa.Article);

            var ordersDetailed = await orders.Select(o => new Order
            {
                Id = o.Id,
                Articles = o.OrderArticles.Select(oa => new OrderArticle
                {
                    Id = oa.Article.Id,
                    Name = oa.Article.Name,
                    Price = oa.Article.Price,
                    Quantity = oa.ArticleQuantity
                })
            }).ToArrayAsync();

            return ordersDetailed;
        }

        public async Task<Order?> Get(int id)
        {
            var order = await _context.Orders
                .Include(o => o.OrderArticles)
                .ThenInclude(oa => oa.Article)
                .FirstOrDefaultAsync(x => x.Id == id);

            if (order == null)
            {
                return null;
            }

            var orderDetailed = new Order
            {
                Id = order.Id,
                Articles = order.OrderArticles.Select(oa => new OrderArticle
                {
                    Id = oa.Article.Id,
                    Name = oa.Article.Name,
                    Price = oa.Article.Price,
                    Quantity = oa.ArticleQuantity
                })
            };

            return orderDetailed;
        }

        public async Task Add(IEnumerable<ArticleInput> inputArticles)
        {
            var orderArticles = inputArticles.Select(x => new OrderArticleDal
            {
                ArticleId = x.Id,
                ArticleQuantity = x.Quantity
            }).ToArray();

            var dbOrder = new OrderDal
            {
                OrderArticles = orderArticles
            };

            _context.Orders.Add(dbOrder);
            await _context.SaveChangesAsync();
        }

        public async Task Update(Order order)
        {
            var orderArticlesDal = order.Articles.Select(x => new OrderArticleDal
            {
                OrderId = order.Id,
                ArticleId = x.Id,
                ArticleQuantity = x.Quantity
            }).ToList();

            await _context.OrderArticles
                .Where(x => x.OrderId == order.Id)
                .ExecuteDeleteAsync();

            _context.OrderArticles.AddRange(orderArticlesDal);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsExist(int id)
        {
            return await _context.Orders.FirstOrDefaultAsync(x => x.Id == id) != null;
        }
    }
}
