using GestionVoituresExpress.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionVoituresExpress.ViewModels
{
    public class RepairingViewModel
    {
        public int? RepairingTypeId { get; set; }
        public string?  Type { get; set; }
        public decimal? Price { get; set; }
        public DateTime? RepairingDate { get; set; }

  
    }
}
