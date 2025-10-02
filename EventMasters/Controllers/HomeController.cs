using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using EventMasters.Models;
using System.Threading.Tasks;
using EventMasters.Data;
using Microsoft.EntityFrameworkCore;

namespace EventMasters.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly EventMastersContext _context;

        // My Home Controller
        public HomeController(ILogger<HomeController> logger, EventMastersContext context)
        {
            _logger = logger;
            _context = context;
           
        }

        //Get: Concerts
        public async Task<IActionResult> Index()
        {
            

            return View();
        }

        //gGET: Concerts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            //url missing 3rs parameter
            if (id == null)
            {
                return NotFound();
            }

            //get record when Prim Key = id
            var concert = await _context.Concert.FirstOrDefaultAsync(mbox => mbox.ConcertId == id);
            
            //record not found in database
            if(concert == null)
            { 
                return NotFound();
            }

            
            
            
            return View(concert);
        }
    }
}
