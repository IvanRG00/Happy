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
    public class RestaurantsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RestaurantsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Restaurants
        public async Task<IActionResult> Index(string sortOrder,
            string searchString,
            string currentfilter,
            int? pagenumber)
        {
            ViewData["CurrentSort"] = sortOrder;
            ViewData["LocationParam"] = sortOrder == "Location" ? "LocationDesc" : "";
            
            var restaurants = from s in _context.Restaurants select s;
            
            
            if (searchString != null)
            {
                pagenumber = 1;
            }
            else
            {
                searchString = currentfilter;
            }
            ViewData["CurrentFilter"] = searchString;
            
            if (!String.IsNullOrEmpty(searchString))
            {
                restaurants = restaurants.Where(s => s.Location.Contains(searchString));
            }
           
            
            switch (sortOrder)
            {
                case "LocationDesc":
                    restaurants.OrderByDescending(s => s.Location);
                    break;
                default:
                    restaurants = restaurants.OrderBy(s => s.Location);
                    break;
            }
            int pageSize = 3;
            return View(await PaginatedList<Restaurants>.CreateAsync(restaurants.AsNoTracking(), pagenumber ?? 1, pageSize));
        }

        // GET: Restaurants/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Restaurants == null)
            {
                return NotFound();
            }

            var restaurants = await _context.Restaurants
                .FirstOrDefaultAsync(m => m.Id == id);
            if (restaurants == null)
            {
                return NotFound();
            }

            return View(restaurants);
        }

        // GET: Restaurants/Create
        [Authorize]
        
        public IActionResult Create()
        {
            return View();
        }

        // POST: Restaurants/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Location,Address,ContactNumber")] Restaurants restaurants)
        {
            if (ModelState.IsValid)
            {
                _context.Add(restaurants);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(restaurants);
        }

        // GET: Restaurants/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Restaurants == null)
            {
                return NotFound();
            }

            var restaurants = await _context.Restaurants.FindAsync(id);
            if (restaurants == null)
            {
                return NotFound();
            }
            return View(restaurants);
        }

        // POST: Restaurants/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Location,Address,ContactNumber")] Restaurants restaurants)
        {
            if (id != restaurants.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(restaurants);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RestaurantsExists(restaurants.Id))
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
            return View(restaurants);
        }
        [Authorize]
        // GET: Restaurants/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Restaurants == null)
            {
                return NotFound();
            }

            var restaurants = await _context.Restaurants
                .FirstOrDefaultAsync(m => m.Id == id);
            if (restaurants == null)
            {
                return NotFound();
            }

            return View(restaurants);
        }

        // POST: Restaurants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Restaurants == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Restaurants'  is null.");
            }
            var restaurants = await _context.Restaurants.FindAsync(id);
            if (restaurants != null)
            {
                _context.Restaurants.Remove(restaurants);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RestaurantsExists(int id)
        {
          return _context.Restaurants.Any(e => e.Id == id);
        }
    }
}
