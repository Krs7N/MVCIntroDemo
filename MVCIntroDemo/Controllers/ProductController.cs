using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Net.Http.Headers;
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

        [ActionName("My-Products")]
        public IActionResult Index(string? keyword = null)
        {
            if (keyword != null)
            {
                var filteredProducts = _products.Where(p => p.Name!.ToLower().Contains(keyword.ToLower()));

                return View(filteredProducts);
            }

            return View(_products);
        }

        public IActionResult Single(int id)
        {
            var model = this._products.FirstOrDefault(p => p.Id == id);

            if (model == null)
            {
                TempData["ErrorMessage"] = "No such product.";
                return RedirectToAction("My-Products");
                //return BadRequest("No such product.");
            }

            return View(model);
        }

        public IActionResult AllAsJson()
        {
            var options = new JsonSerializerOptions()
            {
                WriteIndented = true
            };

            return Json(_products, options);
        }

        public IActionResult AllAsText()
        {
            var sb = new StringBuilder();

            foreach (var product in _products)
            {
                sb.AppendLine($"Product {product.Id}: {product.Name} - {product.Price} lv.");
            }

            return Content(sb.ToString().Trim());
        }

        public IActionResult Download()
        {
            var sb = new StringBuilder();

            foreach (var product in _products)
            {
                sb.AppendLine($"Product {product.Id}: {product.Name} - {product.Price} lv.");
            }

            Response.Headers.Add(HeaderNames.ContentDisposition, @"attachment; filename=products.txt");

            return File(Encoding.UTF8.GetBytes(sb.ToString().Trim()), "text/plain");
        }
    }
}
