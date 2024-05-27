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
    public class SizeController : Controller
    {
        private readonly DataContext _context;

        public SizeController(DataContext context)
        {
            _context = context;
        }

        // GET: Size
        public async Task<IActionResult> SizeList()
        {
            var category = await _context.Size.ToListAsync();
            return View(category);
        }



        public IActionResult AddSize()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddSize(Size size)
        {
                size.Id = Guid.NewGuid();
                _context.Add(size);
                await _context.SaveChangesAsync();
                return RedirectToAction("SizeList");
        }

        // GET: Size/Edit/5
        public async Task<IActionResult> EditSize(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var size = await _context.Size.FindAsync(id);
            if (size == null)
            {
                return NotFound();
            }
            return View(size);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSize(Guid id,  Size size)
        {
            if (id != size.Id)
            {
                return NotFound();
            }

                try
                {
                    _context.Update(size);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SizeExists(size.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("SizeList");
        }

        // GET: Size/Delete/5
        public async Task<IActionResult> DeleteSize(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var size = await _context.Size
                .FirstOrDefaultAsync(m => m.Id == id);
            if (size == null)
            {
                return NotFound();
            }

            return View(size);
        }

        // POST: Size/Delete/5
        [HttpPost, ActionName("DeleteSize")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedSize(Guid id)
        {
            var size = await _context.Size.FindAsync(id);
            if (size != null)
            {
                _context.Size.Remove(size);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("SizeList");
        }

        private bool SizeExists(Guid id)
        {
            return _context.Size.Any(e => e.Id == id);
        }
    }
}
