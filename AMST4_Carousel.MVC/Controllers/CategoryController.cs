using Microsoft.AspNetCore.Mvc;
using AMST4.Carousel.MVC.Context;

namespace AMST4_Carousel.MVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly DataContext _DataContext;
        public CategoryController(DataContext DataContext)
        {
            _DataContext = DataContext;
        }
        public IActionResult CategoryList()
        {
            var categories = _DataContext.Category.ToList();
            return View(categories);
        }
    }
}
