using Store.Data;
using Store.Services.Models;
using Store.ViewModels;

namespace Store.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepo;
        private readonly IArticleRepository _articleRepo;
        private readonly ILogger<OrderService> _logger;

        public OrderService(IOrderRepository orderRepo, IArticleRepository articleRepo, ILogger<OrderService> logger)
        {
            _logger = logger;
            _orderRepo = orderRepo;
            _articleRepo = articleRepo;
        }

        public async Task<IEnumerable<Order>> GetAllOrders()
        {
            try
            {
                return await _orderRepo.GetAll();
            }
            catch (Exception ex)
            {
                var message = "Unable to get order list.";
                _logger.LogError(message, ex.ToString());
                throw new Exception(message);
            }
        }

        public async Task<Order> GetOrder(int id)
        {
            Order order;
            try
            {
                order = await _orderRepo.Get(id);
            }
            catch (Exception ex)
            {
                var message = "Unable to get order.";
                _logger.LogError(message, ex.ToString());
                throw new Exception(message);
            }

            if (order == null)
            {
                throw new Exception($"Order id='{id}' is not exist.");
            }

            return order;
        }

        public async Task CreateOrder(IEnumerable<ArticleInput> inputArticles)
        {
            var existingArticles = await _articleRepo.GetAll();
            if (inputArticles.All(ia => existingArticles.Any(ea => ea.Id == ia.Id)))
            {
                try
                {
                    await _orderRepo.Add(inputArticles);
                }
                catch (Exception ex)
                {
                    var message = "Unable to create order.";
                    _logger.LogError(message, ex.ToString());
                    throw new Exception(message);
                }
            }
            else
            {
                throw new Exception("Some articles already don't exist in database.");
            }
        }

        public async Task UpdateOrder(int id, IList<AddedArticleVm> addedArticles)
        {
            var isOrderExist = await _orderRepo.IsExist(id);
            if (isOrderExist)
            {
                try
                {
                    var order = new Order
                    {
                        Id = id,
                        Articles = addedArticles.Select(x => new OrderArticle
                        {
                            Id = x.Id,
                            Quantity = x.Quantity
                        })
                    };
                    await _orderRepo.Update(order);
                }
                catch (Exception ex)
                {
                    var message = $"Unable to update order '{id}'.";
                    _logger.LogError(message, ex.ToString());
                    throw new Exception(message);
                }
            }
            else
            {
                throw new Exception($"Order '{id}' is not ex");
            }
        }
    }
}
