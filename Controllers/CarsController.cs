using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestionVoituresExpress.Models;

namespace GestionVoituresExpress.Controllers
{
    public class CarsController : Controller
    {
        private readonly AppDbContext _context;

        public CarsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Cars
        public async Task<IActionResult> Index()
        {
              return _context.Cars != null ? 
                          View(await _context.Cars.ToListAsync()) :
                          Problem("Entity set 'AppDbContext.Cars'  is null.");
        }

    
    
        private bool CarExists(int id)
        {
          return (_context.Cars?.Any(e => e.CarID == id)).GetValueOrDefault();
        }
    }
}
