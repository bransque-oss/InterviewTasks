using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Store.ViewModels;

namespace Store.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Redirect("Order");
        }

        public IActionResult Error()
        {
            var exceptionFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
            return View(new ErrorVm
            {
                ErrorMessage = exceptionFeature.Error.Message
            });
        }
    }
}
