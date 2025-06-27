using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
            var cars = await _context.Cars.ToListAsync();

            var viewModels = new List<CarViewModel>();

            foreach (var car in cars)
            {
                var transaction = await _context.Transactions
                    .Where(t => t.CarID == car.CarID)
                    .OrderByDescending(t => t.BuyingDate)
                    .FirstOrDefaultAsync();

                viewModels.Add(new CarViewModel
                {
                    Car = car,
                    Transaction = transaction
                });
            }

            return View(viewModels);
        }

        // GET: Filtrer
        [HttpGet]
        public async Task<IActionResult> Filtrer(string marque, string model, int? annee, decimal? prix)
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

            if (annee.HasValue)
            {
                query = query.Where(c => c.Year == annee);
            }

            var cars = await query.ToListAsync();

            var viewModels = new List<CarViewModel>();

            foreach (var car in cars)
            {
                var transaction = await _context.Transactions
                    .Where(t => t.CarID == car.CarID)
                    .OrderByDescending(t => t.BuyingDate)
                    .FirstOrDefaultAsync();

                // Si filtre prix, exclure si non conforme
                if (prix.HasValue)
                {
                    if (transaction == null || transaction.SellingPrice > prix.Value)
                        continue;
                }

                viewModels.Add(new CarViewModel
                {
                    Car = car,
                    Transaction = transaction
                });
            }

            return View("Index", viewModels);
        }

        // GET: Details
        public async Task<IActionResult> Details(int id)
        {
            var car = await _context.Cars.FindAsync(id);

            if (car == null)
                return NotFound();

            var transaction = await _context.Transactions
                .Where(t => t.CarID == car.CarID)
                .OrderByDescending(t => t.BuyingDate)
                .FirstOrDefaultAsync();

            var vm = new CarViewModel
            {
                Car = car,
                Transaction = transaction
            };

            return View(vm);
        }

        // GET: Edit
        public async Task<IActionResult> Edit(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
                return NotFound();

            var transaction = await _context.Transactions
                .Where(t => t.CarID == car.CarID)
                .OrderByDescending(t => t.BuyingDate)
                .FirstOrDefaultAsync();

            var vm = new CarViewModel
            {
                Car = car,
                Transaction = transaction
            };

            return View(vm);
        }
        public async Task<IActionResult> Contact()
        {
           
            
            return View("Contact");
        }

        // POST: Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CarViewModel vm, IFormFile ImageUpload)
        {
            if (id != vm.Car.CarID)
                return BadRequest();

            if (ImageUpload != null && ImageUpload.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(ImageUpload.FileName);
                var filePath = Path.Combine("wwwroot/img/Voitures", fileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageUpload.CopyToAsync(stream);
                }

                vm.Car.ImageURL = $"img/Voitures/{fileName}";
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(vm.Car);
                    if (vm.Transaction != null)
                    {
                        _context.Update(vm.Transaction);
                    }
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(vm.Car.CarID))
                        return NotFound();
                    else
                        throw;
                }
            }

            return View(vm);
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(c => c.CarID == id);
        }
    }
}
