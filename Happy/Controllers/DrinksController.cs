using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Happy.Data;
using Happy.Models;

namespace Happy.Controllers
{
    public class DrinksController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DrinksController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Drinks
        public async Task<IActionResult> Index(string sortOrder, string searchString)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["DrinkSortParam"] = sortOrder == "Drink" ? "DrinkNameDesc" : "";
            ViewData["mlSortParam"] = sortOrder == "ml" ? "mlDesc" : "ml";
            var drinks= from s in _context.Drinks select s;
            if (!String.IsNullOrEmpty(searchString))
            {
                drinks = drinks.Where(s => s.DrinkName.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "DrinkNameDesc":
                     drinks.OrderByDescending(s => s.DrinkName);
                    break;
                case "ml":
                    drinks = drinks.OrderBy(s => s.ml);
                    break;
                case "mlDesc":
                    drinks = drinks.OrderByDescending(s => s.ml);
                    break;
                default:
                    drinks = drinks.OrderBy(s => s.DrinkName);
                    break;
            }
            return View(await drinks.AsNoTracking().ToListAsync());
        }

        // GET: Drinks/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Drinks == null)
            {
                return NotFound();
            }

            var drinks = await _context.Drinks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (drinks == null)
            {
                return NotFound();
            }

            return View(drinks);
        }

        // GET: Drinks/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Drinks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DrinkName,ingredients,ml")] Drinks drinks)
        {
            if (ModelState.IsValid)
            {
                _context.Add(drinks);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(drinks);
        }

        // GET: Drinks/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Drinks == null)
            {
                return NotFound();
            }

            var drinks = await _context.Drinks.FindAsync(id);
            if (drinks == null)
            {
                return NotFound();
            }
            return View(drinks);
        }

        // POST: Drinks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DrinkName,ingredients,ml")] Drinks drinks)
        {
            if (id != drinks.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(drinks);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DrinksExists(drinks.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(drinks);
        }

        // GET: Drinks/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Drinks == null)
            {
                return NotFound();
            }

            var drinks = await _context.Drinks
                .FirstOrDefaultAsync(m => m.Id == id);
            if (drinks == null)
            {
                return NotFound();
            }

            return View(drinks);
        }

        // POST: Drinks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Drinks == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Drinks'  is null.");
            }
            var drinks = await _context.Drinks.FindAsync(id);
            if (drinks != null)
            {
                _context.Drinks.Remove(drinks);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DrinksExists(int id)
        {
          return _context.Drinks.Any(e => e.Id == id);
        }
    }
}
