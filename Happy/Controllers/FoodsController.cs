using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Happy.Data;
using Happy.Models;
using Microsoft.AspNetCore.Authorization;

namespace Happy.Controllers
{
    public class FoodsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public FoodsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Foods
        public async Task<IActionResult> Index(string sortOrder,string searchString)
        {

            ViewData["MenuSortParam"] = sortOrder == "Menu" ? "MenuNameDesc" : "";
            ViewData["GramsSortParam"] =sortOrder=="Grams" ? "GramsDesc" : "Grams";
            ViewData["SearchParam"] = searchString;

            var foods = from s in _context.Foods select s;

            if (!String.IsNullOrEmpty(searchString))
            {
                foods = foods.Where(s => s.MenuName.Contains(searchString));
            }


            switch (sortOrder) {
                case "MenuNameDesc":
                    foods = foods.OrderByDescending(s => s.MenuName);
                  break;
                case "Grams":
                    foods = foods.OrderBy(s => s.Grams);
                    break;
                case "GramsDesc":
                    foods = foods.OrderByDescending(s => s.Grams);
                    break;
                default:
                    foods = foods.OrderBy(s => s.MenuName);
                    break;
            }


            return View(await foods.AsNoTracking().ToListAsync());
        }

        // GET: Foods/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Foods == null)
            {
                return NotFound();
            }

            var foods = await _context.Foods
                .FirstOrDefaultAsync(m => m.Id == id);
            if (foods == null)
            {
                return NotFound();
            }

            return View(foods);
        }

        // GET: Foods/Create
        [Authorize]
        public IActionResult Create()
        {
            return View();
        }

        // POST: Foods/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,MenuName,ingredients,Grams")] Foods foods)
        {
            if (ModelState.IsValid)
            {
                _context.Add(foods);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(foods);
        }

        // GET: Foods/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Foods == null)
            {
                return NotFound();
            }

            var foods = await _context.Foods.FindAsync(id);
            if (foods == null)
            {
                return NotFound();
            }
            return View(foods);
        }

        // POST: Foods/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MenuName,ingredients,Grams")] Foods foods)
        {
            if (id != foods.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(foods);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FoodsExists(foods.Id))
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
            return View(foods);
        }

        // GET: Foods/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Foods == null)
            {
                return NotFound();
            }

            var foods = await _context.Foods
                .FirstOrDefaultAsync(m => m.Id == id);
            if (foods == null)
            {
                return NotFound();
            }

            return View(foods);
        }

        // POST: Foods/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Foods == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Foods'  is null.");
            }
            var foods = await _context.Foods.FindAsync(id);
            if (foods != null)
            {
                _context.Foods.Remove(foods);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FoodsExists(int id)
        {
          return _context.Foods.Any(e => e.Id == id);
        }
    }
}
