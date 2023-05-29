using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Store.Services;
using Store.Services.Models;
using Store.ViewModels;

namespace Store.Controllers
{
    [Route("[controller]")]
    public class OrderController : Controller
    {
        private static IList<AddedArticleVm> _addedArticles = new List<AddedArticleVm>();
        private static IEnumerable<Article> _availableArticles;
        private static IList<SelectListItem> _availableSelectListArticles;

        private readonly IOrderService _orderService;
        private readonly IArticleService _articleService;

        public OrderController(IOrderService orderService, IArticleService articleService)
        {
            _orderService = orderService;
            _articleService = articleService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var orders = await _orderService.GetAllOrders();
            var vmOrders = orders.Select(x => new OrderSimpleVm
            {
                Id = x.Id,
                Total = x.Articles.Sum(a => a.Price * a.Quantity)
            });

            return View(vmOrders);
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<IActionResult> OrderDetails(int id)
        {
            await CreateAvailableArticleLists();
            var order = await _orderService.GetOrder(id);
            _addedArticles = order.Articles.Select(x => new AddedArticleVm
            {
                Id = x.Id,
                Name = x.Name,
                Price = x.Price,
                Quantity = x.Quantity
            }).ToList();

            var orderVm = new OrderCreateUpdateVm
            {
                Id = id,
                AddedArticles = _addedArticles,
                AvailableArticles = _availableSelectListArticles
            };

            return View(orderVm);
        }

        [HttpGet("create")]
        public async Task<IActionResult> Create()
        {
            await CreateAvailableArticleLists();
            var orderVm = new OrderCreateUpdateVm
            {
                AddedArticles = _addedArticles,
                AvailableArticles = _availableSelectListArticles
            };

            return View(orderVm);
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create(OrderCreateUpdateVm orderDetailsVm)
        {
            if (!ModelState.IsValid)
            {
                FillOrderVmByArticleLists(orderDetailsVm);
                return View(orderDetailsVm);
            }
            if (orderDetailsVm.AddedArticles == null)
            {
                ModelState.AddModelError(string.Empty, "You didn't choose any articles.");
                FillOrderVmByArticleLists(orderDetailsVm);
                return View(orderDetailsVm);
            }

            var inputArticles = orderDetailsVm.AddedArticles.Select(x => new ArticleInput
            {
                Id = x.Id,
                Quantity = x.Quantity
            });
            await _orderService.CreateOrder(inputArticles);
            ClearLists();

            return RedirectToAction("Index");
        }

        [HttpPost("{id:int:min(1)}")]
        public async Task<IActionResult> Update(int id, OrderCreateUpdateVm orderDetailsVm)
        {
            await _orderService.UpdateOrder(id, orderDetailsVm.AddedArticles);
            ClearLists();
            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult AddSelectedArticle(OrderCreateUpdateVm orderDetailsVm)
        {
            string returnViewName;
            if (orderDetailsVm.Id == null)
            {
                returnViewName = "Create";
            }
            else
            {
                returnViewName = "OrderDetails";
            }

            if (!ModelState.IsValid)
            {
                FillOrderVmByArticleLists(orderDetailsVm);
                return View("Create", orderDetailsVm);
            }

            var availableArticle = _availableArticles.First(x => x.Id == orderDetailsVm.ArticleToAdd.Id);
            orderDetailsVm.ArticleToAdd.Name = availableArticle.Name;
            orderDetailsVm.ArticleToAdd.Price = availableArticle.Price;
            var existingArticle = _addedArticles.FirstOrDefault(x => x.Id == orderDetailsVm.ArticleToAdd.Id);
            if (existingArticle == null)
            {
                _addedArticles.Add(orderDetailsVm.ArticleToAdd);
            }
            else
            {
                existingArticle.Quantity += orderDetailsVm.ArticleToAdd.Quantity;
            }
            FillOrderVmByArticleLists(orderDetailsVm);
            orderDetailsVm.HasChanges = true;
            return View(returnViewName, orderDetailsVm);
        }

        private async Task CreateAvailableArticleLists()
        {
            ClearLists();
            _availableArticles = await _articleService.GetArticles();
            _availableSelectListArticles = _availableArticles.Select(x => new SelectListItem(x.Name, x.Id.ToString())).ToList();
        }
        private void FillOrderVmByArticleLists(OrderCreateUpdateVm orderVm)
        {
            orderVm.AddedArticles = _addedArticles;
            orderVm.AvailableArticles = _availableSelectListArticles;
        }
        private void ClearLists()
        {
            _addedArticles.Clear();
            _availableArticles = null;
            _availableSelectListArticles = null;
        }
    }
}
