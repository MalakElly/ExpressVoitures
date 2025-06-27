using GestionVoituresExpress.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace GestionVoituresExpress.ViewModels
{
    public class CarViewModel
    {
        public Car Car { get; set; }
        public Transaction Transaction { get; set; }

        public List<RepairingViewModel>? Repairings { get; set; }
        public List<SelectListItem>? AvailableTypes { get; set; }

    }
}
