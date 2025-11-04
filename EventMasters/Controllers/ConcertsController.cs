using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using EventMasters.Data;
using EventMasters.Models;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;


namespace EventMasters.Controllers
{
    [Authorize] //restricted accwess
    public class ConcertsController : Controller
    {
        private readonly EventMastersContext _context;
        private readonly IConfiguration _configuration;
        private readonly BlobContainerClient _containerClient;


        public ConcertsController(EventMastersContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;

            var connectionString = _configuration["AzureStorage"];
            var containerName = "eventmaster-uploads";
            _containerClient = new BlobContainerClient(connectionString, containerName);
        }

        // to create a dropdown menu//helper method
        private void PopulateCategoryList()
        {
            var cats = _context.Category        
                         .OrderBy(c => c.Name)  
                         .ToList();

            ViewBag.NoCategories = !cats.Any();
            ViewBag.CategoryList = new SelectList(cats, "CategoryId", "Name"); // this is the little dropdown
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
        public async Task<IActionResult> Create(
        [Bind("ConcertId,Title,Description,Location,Owner,DateAdded,EventDate,Category,ImageFile")] Concert concert,
        int? CategoryId)
            
        {
            //marks this as added today
            concert.DateAdded = DateTime.Now;


            // If a category was chosen, attach it
            if (CategoryId.HasValue)
            {
                var cat = await _context.Category.FindAsync(CategoryId.Value);
                if (cat != null) concert.Category = cat.Name;
            }


            if (ModelState.IsValid)//check validation
            {

                if (concert.ImageFile != null)
                {
                    string blobName = Guid.NewGuid().ToString() + "_" + concert.ImageFile.FileName;

                    //blob client for that file
                    var blobClient = _containerClient.GetBlobClient(blobName);

                    //Upload to Blob
                    using (var stream = concert.ImageFile.OpenReadStream())
                    {
                        await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = concert.ImageFile.ContentType });
                    }
                    concert.Filename = blobClient.Uri.ToString();
                }

                    
               
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

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ConcertId,Title,Description,Location,Owner,DateAdded,EventDate,Category,Filename,ImageFile")] Concert concert)
        {
            if (id != concert.ConcertId)
                return NotFound();

            if (ModelState.IsValid)
            {
                var existing = await _context.Concert.AsNoTracking().FirstOrDefaultAsync(c => c.ConcertId == id);
                if (existing == null)
                    return NotFound();

                // Handle new image upload
                if (concert.ImageFile != null)
                {
                    string blobName = Guid.NewGuid().ToString() + "_" + concert.ImageFile.FileName;

                    //blob client for that file
                    var blobClient = _containerClient.GetBlobClient(blobName);

                    //Upload to Blob
                    using (var stream = concert.ImageFile.OpenReadStream())
                    {
                        await blobClient.UploadAsync(stream, new BlobHttpHeaders { ContentType = concert.ImageFile.ContentType });
                    }
                    concert.Filename = blobClient.Uri.ToString();
                }
                else
                {
                    // If no new file, keep the old filename
                    concert.Filename = existing.Filename;
                }

                _context.Update(concert);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home");
            }

            return View(concert);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteImage(int id)
        {
            var concert = await _context.Concert.FindAsync(id);
            if (concert == null)
                return NotFound();

            if (!string.IsNullOrEmpty(concert.Filename))
            {
                // Extract blob name 
                var blobUri = new Uri(concert.Filename);
                var blobName = Uri.UnescapeDataString(blobUri.Segments.Last());

                // Delete blob 
                var blobClient = _containerClient.GetBlobClient(blobName);
                var deleted = await blobClient.DeleteIfExistsAsync();

                // Only clear database record if blob was actually deleted
                if (deleted)
                {
                    concert.Filename = null;
                    _context.Update(concert);
                    await _context.SaveChangesAsync();
                }
            }

            // Return to the same Edit view
            return RedirectToAction("Edit", new { id = concert.ConcertId });
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
