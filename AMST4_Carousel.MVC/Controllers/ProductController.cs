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
    public class ProductController : Controller
    {
        private readonly DataContext _context;

        public ProductController(DataContext context)
        {
            _context = context;
        }

        //List
        public async Task<IActionResult> ProductList()
        {
            var dataContext = _context.Product.Include(p => p.Category);
            return View(await dataContext.ToListAsync());
        }

        //Começo Details
        public async Task<IActionResult> DetailsProduct(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }
        //Fim Details
        //Começo Create
        public IActionResult AddProduct()
        {
            ViewBag.Categorylist = new SelectList(_context.Category, "Id", "Description");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddProduct(Product product, IFormFile image)
        {
            if (image != null)
            {
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Product", fileName);
                using (var stream = new FileStream(filepath, FileMode.Create))
                {
                    await image.CopyToAsync(stream);
                }
                product.ImageUrl = Path.Combine("images", "Product", fileName);
            }
            product.Id = Guid.NewGuid();
            _context.Add(product);
            await _context.SaveChangesAsync();
            TempData["ToastType"] = "success";
            TempData["ToastMessage"] = "Produto adicionado com sucesso!";
            return RedirectToAction("ProductList");

        }
        //Fim Create
        //Começo Edit        
        public async Task<IActionResult> EditProduct(Guid? id)
        {
            ViewBag.Categorylist = new SelectList(_context.Category, "Id", "Description");
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(Guid? id, Product product, IFormFile image)
        {
            if (id != product.Id)
            {
                return NotFound();
            }
                try
                {
                    if (image != null)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                        var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Product", fileName);
                        using (var stream = new FileStream(filepath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }
                        // Delete old image if exists
                        if (!string.IsNullOrEmpty(product.ImageUrl))
                        {
                            var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", product.ImageUrl);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }
                        product.ImageUrl = Path.Combine("images", "Product", fileName);
                    }

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ProductList));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProduct(Guid id, [Bind("Id,Name,Description,ImageUrl,CategoryId")] Product product, IFormFile image)
        {
            if (id != product.Id)
            {
                return NotFound();
            }
                try
                {
                    if (image != null)
                    {
                        var fileName = Guid.NewGuid().ToString() + Path.GetExtension(image.FileName);
                        var filepath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "Product", fileName);
                        using (var stream = new FileStream(filepath, FileMode.Create))
                        {
                            await image.CopyToAsync(stream);
                        }
                        // Delete old image if exists
                        if (!string.IsNullOrEmpty(product.ImageUrl))
                        {
                            var oldImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", product.ImageUrl);
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }
                        product.ImageUrl = Path.Combine("images", "Product", fileName);
                    }

                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            TempData["ToastType"] = "success";
            TempData["ToastMessage"] = "Produto Editado com sucesso!";
            return RedirectToAction(nameof(ProductList));
        }

        //Fim Edit
        //Começo Delete
        public async Task<IActionResult> DeleteProduct(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }
            TempData["ToastType"] = "warning";
            TempData["ToastMessage"] = "Tem Certeza que deseja excluir isso?";
            return View(product);
        }

        [HttpPost, ActionName("DeleteProduct")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedProduct(Guid id)
        {
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                // Delete the image if it exists
                if (!string.IsNullOrEmpty(product.ImageUrl))
                {
                    var imagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", product.ImageUrl);
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }

                _context.Product.Remove(product);
            }

            await _context.SaveChangesAsync();
            TempData["ToastType"] = "success";
            TempData["ToastMessage"] = "Produto excluído com sucesso!";
            return RedirectToAction(nameof(ProductList));
        }

        private bool ProductExists(Guid id)
        {
            return _context.Product.Any(e => e.Id == id);
        }
        //Fim Delete
    }
}
