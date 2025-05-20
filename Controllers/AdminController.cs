using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionVoituresExpress.Models;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using GestionVoituresExpress.ViewModels;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        //Récupère l'id de l'admin
        private string GetAdminUserId()
        {
            var admin = _context.Users.FirstOrDefault(u => u.IsAdmin == true);
            return admin != null ? admin.Id : throw new Exception("Aucun admin trouvé.");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Index()
        {
            var Cars = await _context.Cars.ToListAsync();
            return View(Cars);
        }

        public IActionResult Create()
        {
            var types = _context.RepairingType
                .Select(rt => new SelectListItem
                {
                    Value = rt.RepairingTypeId.ToString(),
                    Text = rt.RepairingName
                }).ToList();

            var vm = new CarViewModel
            {
                Car = new Car(),
                Transaction = new Transaction(),
                Repairings = new List<RepairingViewModel>
        {
            new RepairingViewModel
            {
                AvailableTypes = types
            }
        }
            };

            return View(vm);
        }

        [HttpPost]
     
        public async Task<IActionResult> Create(Transaction transaction,Car car,List<Repairing> Repairings, IFormFile ImageUpload)
        {

            transaction.User = _context.Users.FirstOrDefault(u => u.IsAdmin == true);
            transaction.UserID = transaction.User.Id;
            transaction.Car.Repairing = Repairings;
            
            //Generer via GUID pour le id Car
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

               
                transaction.Car.ImageURL = $"img/Voitures/{fileName}";
            }
            if (ModelState.IsValid)
            {
                if (transaction.SellingPrice == null) 
                {
                    foreach (var repairing in Repairings)
                    {
                        transaction.SellingPrice += repairing.RepairingPrice;
                    }

                    transaction.SellingPrice = transaction.BuyingPrice + 500 + transaction.SellingPrice;//prix initial avec coût réparations
                }

                //_context.Repairing.AddRange(transaction.Car.Repairing);
                //_context.Cars.Add(transaction.Car);
                _context.Transactions.Add(transaction);
                await _context.SaveChangesAsync();
                return RedirectToAction("List","Admin");
            }
            else
                { 
                //TODO/FichierLOg
                foreach (var modelState in ModelState)
                {
                    foreach (var error in modelState.Value.Errors)
                    {
                        Console.WriteLine($"Champ : {modelState.Key} — Erreur : {error.ErrorMessage}");
                    }
                }
            }
            return View(transaction);
        }


  

        public async Task<IActionResult> List()
        {
           var transactions = await _context.Transactions
            .Include(t => t.Car)
            .ToListAsync();
             return View(transactions);
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
        public async Task<IActionResult> Edit(int id, Car car,IFormFile ImageUpload)
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
