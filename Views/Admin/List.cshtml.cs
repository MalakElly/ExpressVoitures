using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using GestionVoituresExpress.Models;

namespace GestionVoituresExpress.Views.Admin
{
    public class ListModel : PageModel
    {
        private readonly GestionVoituresExpress.Models.AppDbContext _context;

        public ListModel(GestionVoituresExpress.Models.AppDbContext context)
        {
            _context = context;
        }

        public IList<Car> Car { get;set; } = default!;

        public async Task OnGetAsync()
        {
            if (_context.Cars != null)
            {
                Car = await _context.Cars.ToListAsync();
            }
        }
    }
}
