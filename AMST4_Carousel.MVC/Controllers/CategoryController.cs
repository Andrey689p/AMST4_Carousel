using Microsoft.AspNetCore.Mvc;
using AMST4.Carousel.MVC.Context;
using AMST4_Carousel.MVC.Models;

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
        [HttpPost]
        public IActionResult AddCategory(Category category)
        {
            _DataContext.Category.Add(category);
            _DataContext.SaveChanges();
            category.Id = new Guid();
            return RedirectToAction("CategoryList");
        }
        public IActionResult AddCategory()
        {
            return View();
        }


        [HttpGet]
        public IActionResult DeleteCategory(Guid id)
        {
            var category = _DataContext.Category.Find(id);
            if (category == null)
            {
                return NotFound(); 
            }
            return View(category); 
        }

        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategoryConfirmed(Guid id)
        {
            var category = await _DataContext.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _DataContext.Category.Remove(category);
            await _DataContext.SaveChangesAsync();

            return RedirectToAction("CategoryList");
        }
        }

    }
