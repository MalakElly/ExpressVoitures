using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestionVoituresExpress.Models;
using GestionVoituresExpress.ViewModels;

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
        [HttpPost]
        public async Task<IActionResult> Filtrer(string marque, string model, string annee, string? prix)
        {
           
            var query = _context.Cars.AsQueryable();

            if (!string.IsNullOrEmpty(marque))
            {
                query = query.Where(c => c.Brand.Contains(marque));
            }

            if (!string.IsNullOrEmpty(model))
            {
                query = query.Where(c => c.Model.Contains(model));
            }

            if (!string.IsNullOrEmpty(annee))
            {
                if (int.TryParse(annee, out int year))
                {
                    query = query.Where(c => c.Year <= year);
                }
            }

           if (!string.IsNullOrEmpty(prix))
            {
                if (decimal.TryParse(prix, out decimal price))
                {
                    var carIds = await _context.Transactions
                    .Where(t => t.SellingPrice <= price)
                    .Select(t => t.CarID)
                    .ToListAsync();

                    query = query.Where(c => carIds.Contains(c.CarID));
                }
               
            }


            var result = await query.ToListAsync();
            return View("Index", result); 
        }


        public async Task<IActionResult> Details(int id)
        {
            
            var transaction = await _context.Transactions
                .Include(t=>t.Car)
                .Where(c=>c.CarID==id)
                .FirstOrDefaultAsync();

            if (transaction == null)
            {
                return NotFound();
            }
            CarViewModel carViewModel = new CarViewModel
            {
                Car = transaction.Car,
                Transaction = transaction

            };

            return View(carViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Details(int id, Car car, IFormFile ImageUpload)
        {

            if (id != car.CarID)
            {
                return BadRequest();
            }
            if (ImageUpload != null && ImageUpload.Length > 0)
            {
                // Génèration d'un nom de fichier unique
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(ImageUpload.FileName);
                var filePath = Path.Combine("wwwroot/img/Voitures", fileName);

                // Sauvegarde physique du fichier
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageUpload.CopyToAsync(stream);
                }


                car.ImageURL = $"img/Voitures/{fileName}";
            }
            if (ModelState.IsValid)
            {
                _context.Update(car);
                await _context.SaveChangesAsync();
                return RedirectToAction("List");
            }
            return View(car);
        }
        private bool CarExists(int id)
        {
          return (_context.Cars?.Any(e => e.CarID == id)).GetValueOrDefault();
        }
    }
}
