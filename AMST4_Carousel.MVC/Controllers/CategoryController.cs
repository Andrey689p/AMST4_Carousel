using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AMST4.Carousel.MVC.Context;
using AMST4_Carousel.MVC.Models;

namespace AMST4_Carousel.MVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly DataContext _context;

        public CategoryController(DataContext context)
        {
            _context = context;
        }

        //List 
        public async Task<IActionResult> CategoryList()
        {
            return View(await _context.Category.ToListAsync());
        }

        //Começo Details
        public async Task<IActionResult> DetailsCategory(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }
        //Fim Details
        //Começo Create
        public IActionResult AddCategory()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddCategory(Category category, IFormFile image)
        {
            if (image != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Category", fileName);
                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
                category.ImageUrl = Path.Combine("images", "Category", fileName);
            }
            category.Id = Guid.NewGuid();
            _context.Add(category);
            await _context.SaveChangesAsync();
            TempData["ToastType"] = "success";
            TempData["ToastMessage"] = "Categoria adicionada com sucesso!";
            return RedirectToAction("CategoryList");

        }
        //Fim Create
        //Começo Edit
        public async Task<IActionResult> EditCategory(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditCategory(Guid id, Category category, IFormFile image)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            var existingCategory = await _context.Category.FindAsync(id);
            if (existingCategory == null)
            {
                return NotFound();
            }

            if (image != null)
            {
                // Exclui a imagem Antiga
                if (!string.IsNullOrEmpty(existingCategory.ImageUrl))
                {
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", existingCategory.ImageUrl);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }
                }

                // Salva a Nova Imagem
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                var newFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Category", fileName);
                using (var stream = new FileStream(newFilePath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
                existingCategory.ImageUrl = Path.Combine("images", "Category", fileName);
            }

            // Atualiza outras propriedades da categoria
            existingCategory.Description = category.Description;

            try
            {
                _context.Update(existingCategory);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(existingCategory.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            TempData["ToastType"] = "success";
            TempData["ToastMessage"] = "Categoria editada com sucesso!";
            return RedirectToAction("CategoryList");
        }

        //Fim Edit
        //Começo Delete
        public async Task<IActionResult> DeleteCategory(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var category = await _context.Category
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            TempData["ToastType"] = "warning";
            TempData["ToastMessage"] = "Tem certeza que deseja excluir isso?!";
            return View(category);
        }

        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedCategory(Guid id)
        {
            var category = await _context.Category.FindAsync(id);
            if (category != null)
            {
                // Verifica se a categoria tem uma imagem associada
                if (!string.IsNullOrEmpty(category.ImageUrl))
                {
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", category.ImageUrl);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                // Remove a categoria do banco de dados
                _context.Category.Remove(category);
                await _context.SaveChangesAsync();
            }
            TempData["ToastType"] = "success";
            TempData["ToastMessage"] = "Categoria excluida com sucesso!";
            return RedirectToAction("CategoryList");
        }

        private bool CategoryExists(Guid id)
        {
            return _context.Category.Any(e => e.Id == id);
        }
        //Fim Delete
    }
}
