using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EventMasters.Data;
using EventMasters.Models;

namespace EventMasters.Controllers
{
    public class ConcertsController : Controller
    {
        private readonly EventMastersContext _context;

        public ConcertsController(EventMastersContext context)
        {
            _context = context;
        }

        // to create a dropdown menu//helper method
        private void PopulateCategoryList()
        {
            var cats = _context.Category        
                         .OrderBy(c => c.Name)  
                         .ToList();

            ViewBag.NoCategories = !cats.Any();
            ViewBag.CategoryList = new SelectList(cats, "CategoryId", "Name"); // or "Title" if you use that. this is the little dropdown
        }

        // GET: /Concerts
        public async Task<IActionResult> Index()
        {
            var concerts = await _context.Concert.ToListAsync();//loads into index async
            return View(concerts);//runs when you visit concerts
        }




        // GET: Concerts/Create
        public IActionResult Create()
        {

            ViewBag.CategoryList = new SelectList( //builds the category dropdown
                _context.Set<Category>().OrderBy(c => c.Name).ToList(),
                "CategoryId",   // value
                "Name"          // text shown in dropdown
        );

            return View();
        }

        // POST: Concerts/Create
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ConcertId,Title,Description,Location,Owner,DateAdded,EventDate,Category")] Concert concert,
            int? CategoryId
            )
        {
            //marks this as added today
            concert.DateAdded = DateTime.Now;


            // If a category was chosen, attach it (no model change needed)
            if (CategoryId.HasValue)
            {
                var cat = await _context.Category.FindAsync(CategoryId.Value);
                if (cat != null) concert.Category = cat.Name;
            }


            if (ModelState.IsValid)//checks validation
            {
                _context.Add(concert);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", "Home");
            }

            // rebuild dropdown on error
            ViewBag.CategoryList = new SelectList(
                _context.Set<Category>().OrderBy(c => c.Name).ToList(),
                "CategoryId", "Name", CategoryId
            );
            return View(concert);
        }

        // GET: Concerts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concert = await _context.Concert.FindAsync(id);

            if (concert == null)
            {
                return NotFound();
            }
            return View(concert);
        }

        // POST: Concerts/Edit/5
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ConcertId,Title,Description,Location,Owner,DateAdded,EventDate,Category")] Concert concert)
        {
            if (id != concert.ConcertId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(concert);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ConcertExists(concert.ConcertId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Home");
            }
            return View(concert);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concert = await _context.Concert
                .FirstOrDefaultAsync(m => m.ConcertId == id);

            if (concert == null)
            {
                return NotFound();
            }

            return View(concert);
        }

        // GET: Concerts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var concert = await _context.Concert
                .FirstOrDefaultAsync(m => m.ConcertId == id);

            if (concert == null)
            {
                return NotFound();
            }

            return View(concert);
        }

        // POST: Concerts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var concert = await _context.Concert.FindAsync(id);
            if (concert != null)
            {
                _context.Concert.Remove(concert);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Home");
        }



        private bool ConcertExists(int id)
        {
            return _context.Concert.Any(e => e.ConcertId == id);
        }
    }
}
