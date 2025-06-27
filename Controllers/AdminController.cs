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
        private List<SelectListItem> GetRepairTypes()
        {
            return new List<SelectListItem>
    {
        new SelectListItem { Text = "Vidange", Value = "Vidange" },
        new SelectListItem { Text = "Freins", Value = "Freins" },
        new SelectListItem { Text = "Climatisation", Value = "Clim" },
        new SelectListItem { Text = "Pneus", Value = "Pneus" },
        new SelectListItem { Text = "Moteur", Value = "Moteur" }
    };
        }
        public IActionResult Create()
        {


            var types = GetRepairTypes();
            var vm = new CarViewModel
            {
                Car = new Car(),
                Transaction = new Transaction(),
                Repairings = new List<RepairingViewModel>(),
                AvailableTypes = types
            };

            return View(vm);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CarViewModel vm, IFormFile? ImageUpload)
        {
            var currentUser = await _context.Users.FirstOrDefaultAsync(u => u.IsAdmin);
            if (currentUser != null)
            {
                vm.Transaction.UserID = currentUser.Id;
                vm.Transaction.User = currentUser;
            }

            if (!ModelState.IsValid)
            {
                foreach (var entry in ModelState)
                {
                    foreach (var error in entry.Value.Errors)
                    {
                        Console.WriteLine($"Champ : {entry.Key} - Erreur : {error.ErrorMessage}");
                    }
                }

                vm.AvailableTypes = GetRepairTypes();
                ViewData["ErrorModel"] = "1";
                return View(vm);
            }

            if (ImageUpload != null && ImageUpload.Length > 0)
            {
                var uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images");
                Directory.CreateDirectory(uploadsFolder);

                var uniqueFileName = Guid.NewGuid() + Path.GetExtension(ImageUpload.FileName);
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await ImageUpload.CopyToAsync(stream);
                }

                vm.Car.ImageURL = "/images/" + uniqueFileName;
            }

            using var dbTransaction = await _context.Database.BeginTransactionAsync();
            try
            {
                //Ajouter la voiture
             
                _context.Cars.Add(vm.Car);
                await _context.SaveChangesAsync();

                //  Ajouter les réparations valides
                var reparationsValides = vm.Repairings
                    .ToList();

              
                foreach (var repair in reparationsValides)
                {
                    var repairing = new Repairing
                    {
                        CarID = vm.Car.CarID,
                        RepairingDate = repair.RepairingDate.Value,
                        RepairingPrice = repair.Price.Value,
                       
                    };
                    _context.Repairing.Add(repairing);

                    RepairingType repairingType;
                    var existingType = await _context.RepairingType.FirstOrDefaultAsync(rt => rt.RepairingName == repair.Type);
                    if (existingType == null)
                    {
                        repairingType = new RepairingType { RepairingName = repair.Type };
                        _context.RepairingType.Add(repairingType);
                    }
                    else
                    {
                        repairingType = existingType;
                    }

                    var repairingAndType = new RepairingAndType
                    {
                        Repairing = repairing,
                        RepairingType = repairingType
                    };

                    _context.RepairingAndType.Add(repairingAndType);
                }

                //  Ajouter la transaction

                vm.Transaction.CarID = vm.Car.CarID;
         
                decimal totalRepairs = reparationsValides.Sum(r => r.Price.Value);

            
                vm.Transaction.CarID = vm.Car.CarID;
                if (vm.Transaction.SellingPrice == null)
                {
                    vm.Transaction.SellingPrice = vm.Transaction.BuyingPrice + totalRepairs + 500;
                }
               

            

                
                _context.Transactions.Add(vm.Transaction);

                await _context.SaveChangesAsync();


                await dbTransaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await dbTransaction.RollbackAsync();
                ViewData["errorCodeVin"] = ex.InnerException?.Message ?? ex.Message;
                vm.AvailableTypes = GetRepairTypes();
                return View(vm);
            }

            return RedirectToAction("List");
        }



        public async Task<IActionResult> List()
        {
            var data = await _context.Transactions
                .Include(t => t.Car)
                .Include(t => t.Car.Repairing)
                    .ThenInclude(r => r.RepairingAndTypes)
                    .ThenInclude(rt => rt.RepairingType)
                .Include(t => t.Car) 
                .ToListAsync();

            var vmList = data.Select(t => new CarViewModel
            {
                Car = t.Car,
                Transaction = t,
                Repairings = t.Car.Repairing?
                    .Select(r => new RepairingViewModel
                    {
                        RepairingTypeId = r.RepairingAndTypes.FirstOrDefault()?.RepairingTypeId,
                        Type = r.RepairingAndTypes.FirstOrDefault()?.RepairingType.RepairingName,
                        Price = r.RepairingPrice,
                        RepairingDate = r.RepairingDate
                    })
                    .ToList(),
           
            }).ToList();

            return View(vmList);
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var transaction = await _context.Transactions
                .Include(t => t.Car)
                .FirstOrDefaultAsync(t => t.TransactionID == id);

            if (transaction == null)
                return NotFound();

            var car = transaction.Car;

            var repairs = await _context.Repairing
                .Where(r => r.CarID == car.CarID)
                .Include(r => r.RepairingAndTypes)
                .Select(r => new RepairingViewModel
                {
                    RepairingTypeId = r.RepairingAndTypes.FirstOrDefault().RepairingTypeId, 
                    Price = r.RepairingPrice,
                    RepairingDate = r.RepairingDate
                }).ToListAsync();

            var vm = new CarViewModel
            {
                Car = car,
                Transaction = transaction,
                Repairings = repairs,
                AvailableTypes = GetRepairTypes()
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CarViewModel vm, IFormFile? ImageUpload)
        {
            if (!ModelState.IsValid)
            {
                // Log des erreurs de validation
                foreach (var entry in ModelState)
                    foreach (var error in entry.Value.Errors)
                        Console.WriteLine($"Champ : {entry.Key} - Erreur : {error.ErrorMessage}");

                vm.AvailableTypes = GetRepairTypes();
                return View(vm);
            }

            var car = await _context.Cars.FindAsync(vm.Car.CarID);
            var transaction = await _context.Transactions
                .FirstOrDefaultAsync(t => t.CarID == vm.Car.CarID);

            if (car == null || transaction == null)
                return NotFound();

            // Mise à jour des propriétés de la voiture
            car.CodeVIN = vm.Car.CodeVIN;
            car.Brand = vm.Car.Brand;
            car.Model = vm.Car.Model;
            car.Year = vm.Car.Year;
            car.Trim = vm.Car.Trim;
            car.Km = vm.Car.Km;

            // Image si uploadée
            if (ImageUpload != null && ImageUpload.Length > 0)
            {
                var fileName = Guid.NewGuid() + Path.GetExtension(ImageUpload.FileName);
                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", fileName);
                using var stream = new FileStream(path, FileMode.Create);
                await ImageUpload.CopyToAsync(stream);
                car.ImageURL = "/images/" + fileName;
            }

            // Mise à jour de la transaction
            transaction.BuyingDate = vm.Transaction.BuyingDate;
            transaction.BuyingPrice = vm.Transaction.BuyingPrice;
            transaction.SellingDate = vm.Transaction.SellingDate;
            transaction.SellingPrice = vm.Transaction.SellingPrice;
            transaction.IsAvailable = vm.Transaction.IsAvailable;

            // Gestion des réparations existantes et nouvelles
            using var dbTransaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Suppression des anciennes réparations et associations
                var oldRepairs = _context.Repairing.Where(r => r.CarID == car.CarID);
                _context.RepairingAndType.RemoveRange(
                    _context.RepairingAndType.Where(rt => oldRepairs.Select(r => r.RepairingId).Contains(rt.RepairingId))
                );
                _context.Repairing.RemoveRange(oldRepairs);

                await _context.SaveChangesAsync();

                // Réinsertion des réparations valides
                var reparationsValides = vm.Repairings
                    .Where(r => r.Price.HasValue && r.RepairingDate.HasValue )
                    .ToList();

                foreach (var r in reparationsValides)
                {
                    var repairing = new Repairing
                    {
                        CarID = car.CarID,
                        RepairingPrice = r.Price.Value,
                        RepairingDate = r.RepairingDate.Value
                    };
                    _context.Repairing.Add(repairing);
                    await _context.SaveChangesAsync(); // pour récupérer RepairingId

                    _context.RepairingAndType.Add(new RepairingAndType
                    {
                        RepairingId = repairing.RepairingId,
                        RepairingTypeId = r.RepairingTypeId.Value
                    });
                }

       
                await _context.SaveChangesAsync();
                await dbTransaction.CommitAsync();

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                await dbTransaction.RollbackAsync();
                ViewData["errorCode"] = ex.InnerException?.Message ?? ex.Message;
                vm.AvailableTypes = GetRepairTypes();
                return View(vm);
            }
        }



        // GET pour afficher la confirmation de suppression
        public async Task<IActionResult> Delete(int id)
        {
            var transaction = await _context.Transactions
                .Include(t => t.Car)
                .FirstOrDefaultAsync(t => t.TransactionID == id);

            if (transaction == null)
                return NotFound();

            // Transmettre au ViewModel
            var vm = new CarViewModel
            {
                Car = transaction.Car,
                Transaction = transaction,
                Repairings = await _context.Repairing
                    .Where(r => r.CarID == transaction.CarID)
                    .Select(r => new RepairingViewModel
                    {
                        RepairingTypeId = r.RepairingAndTypes
                            .Select(rt => rt.RepairingTypeId).FirstOrDefault(),
                        Type = r.RepairingAndTypes
                            .Select(rt => rt.RepairingType.RepairingName).FirstOrDefault(),
                        Price = r.RepairingPrice,
                        RepairingDate = r.RepairingDate
                    })
                    .ToListAsync()
            };

            return View(vm); 
        }

        // POST après la confirmation
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var transaction = await _context.Transactions
                .Include(t => t.Car)
                .FirstOrDefaultAsync(t => t.TransactionID == id);

            if (transaction == null)
                return NotFound();

            using var dbTransaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var car = transaction.Car;
                var repairs = _context.Repairing.Where(r => r.CarID == car.CarID);

                // Supprimer RepairingAndType
                var ids = repairs.Select(r => r.RepairingId);
                var rts = _context.RepairingAndType.Where(rt => ids.Contains(rt.RepairingId));
                _context.RepairingAndType.RemoveRange(rts);

                // Supprimer les réparations
                _context.Repairing.RemoveRange(repairs);

                // Supprimer transaction
                _context.Transactions.Remove(transaction);

                // Supprimer voiture
                _context.Cars.Remove(car);

                await _context.SaveChangesAsync();
                await dbTransaction.CommitAsync();
            }
            catch
            {
                await dbTransaction.RollbackAsync();
                ModelState.AddModelError("", "Erreur lors de la suppression.");
                return View(); 
            }

            return RedirectToAction("List");
        }

    }
}
