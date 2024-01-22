using Microsoft.AspNetCore.Mvc;
using MVCIntroDemo.Models;

namespace MVCIntroDemo.Controllers
{
    public class ProductController : Controller
    {
        private readonly ILogger<ProductController> _logger;

        private IEnumerable<ProductViewModel> _products = new List<ProductViewModel>()
        {
            new ProductViewModel() { Id = 1, Name = "Bread", Price = 1.99m },
            new ProductViewModel() { Id = 2, Name = "Cheese", Price = 2.99m },
            new ProductViewModel() { Id = 3, Name = "Ham", Price = 3.99m },
            new ProductViewModel() { Id = 4, Name = "Beer", Price = 4.99m }
        };

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View(_products);
        }

        public IActionResult Single(int id)
        {
            var model = this._products.FirstOrDefault(p => p.Id == id);

            if (model == null)
            {
                TempData["ErrorMessage"] = "No such product.";
                return RedirectToAction("Index");
                //return BadRequest("No such product.");
            }

            return View(model);
        }
    }
}
