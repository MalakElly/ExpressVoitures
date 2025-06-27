using GestionVoituresExpress.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Newtonsoft.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace GestionVoituresExpress.Models
{
    [Index(nameof(CodeVIN), IsUnique = true)]
    public class Car
    {
        [Key]
        public int CarID { get; set; }

        [Required(ErrorMessage = "Le Code VIN est obligatoire.")]
        [StringLength(17, MinimumLength = 17, ErrorMessage = "Le code VIN doit contenir exactement 17 caractères.")]
        [RegularExpression(@"^[A-HJ-NPR-Z0-9]{17}$", ErrorMessage = "Le code VIN n'est pas valide.")]
        public string CodeVIN { get; set; }
        [Required(ErrorMessage = "L'année de production est obligatoire.")]
        [Range(1990, 2100, ErrorMessage = "L'année doit être comprise entre 1990 et 2100.")]
        public int Year { get; set; }

        [Required(ErrorMessage = "La champ de la marque de la voiture est obligatoire.")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Le champ du modèle de la voiture est obligatoire.")]
        public string Model { get; set; }

        [Display(Name = "Finition")]
        public string Trim { get; set; } // Finition

       

        [Range(0, int.MaxValue, ErrorMessage = "Le kilométrage doit être un nombre positif.")]
        public int? Km { get; set; }



        public string? ImageURL { get; set; }

        public ICollection<Repairing>? Repairing { get; set; }
    
    }
}

