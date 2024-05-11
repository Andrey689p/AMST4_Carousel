using AMST4.Carousel.MVC.Context;
using AMST4_Carousel.MVC.Models;
using Microsoft.AspNetCore.Mvc;

namespace AMST4_Carousel.MVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly DataContext _DataContext;
        public ProductController(DataContext DataContext)
        {
            _DataContext = DataContext;
        }
        public IActionResult ProductList()
        {
            var products = _DataContext.Product.ToList();
            return View(products);
        }
        [HttpPost]
        public IActionResult AddProduct(Product product)
        {
            _DataContext.Product.Add(product);
            _DataContext.SaveChanges();
            product.Id = new Guid();
            return RedirectToAction("ProductList");
        }
        public IActionResult AddProduct()
        {
            return View();
        }
        [HttpGet]
        public IActionResult DeleteProduct(Guid id)
        {
            var product = _DataContext.Product.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("DeleteProduct")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteProductConfirmed(Guid id)
        {
            var product = await _DataContext.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _DataContext.Product.Remove(product);
            await _DataContext.SaveChangesAsync();

            return RedirectToAction("ProductList");
        }
    }


}

