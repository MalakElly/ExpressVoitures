using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionVoituresExpress.ViewModels
{
    public class RepairingViewModel
    {
        public string  Type { get; set; }
        public decimal Price { get; set; }
        public DateTime RepairingDate { get; set; }

        public List<SelectListItem>? AvailableTypes { get; set; } 
    }
}
