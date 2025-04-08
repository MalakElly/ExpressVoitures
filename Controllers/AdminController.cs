using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionVoituresExpress.Models;
using System.Threading.Tasks;

namespace GestionCarsExpress.Controllers
{
    [Authorize(Roles = "Admin")] //OnlyAdmins
    public class AdminController : Controller
    { 
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        //  Liste des Voitures

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index()
        {
            var Cars = await _context.Cars.ToListAsync();
            return View(Cars);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Car car)
        {   //Generer via GUID pour le code VIN 

            if (ModelState.IsValid)
            {
                _context.Cars.Add(car);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }

    
        public async Task<IActionResult> Edit(int id)
        {
            var Car = await _context.Cars.FindAsync(id);
            if (Car == null)
            {
                return NotFound();
            }
            return View(Car);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Car car)
        {
            if (id != car.CarID)
            {
                return BadRequest();
            }

            if (ModelState.IsValid)
            {
                _context.Update(car.CarID);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(car);
        }


        public async Task<IActionResult> Delete(int id)
        {
            var Car = await _context.Cars.FindAsync(id);
            if (Car == null)
            {
                return NotFound();
            }
            return View(Car);
        }


        [HttpPost, ActionName("DeleteCar")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCarConfirm(int id)
        {
            var Car = await _context.Cars.FindAsync(id);
            if (Car != null)
            {
                _context.Cars.Remove(Car);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
