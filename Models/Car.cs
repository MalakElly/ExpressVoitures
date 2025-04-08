using GestionVoituresExpress.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace GestionVoituresExpress.Models
{
    public class Car
    {
        [Key]
        public int CarID { get; set; }

        [Required]
        public string CodeVIN { get; set; }

        [Range(1990, 2025, ErrorMessage = "L'année doit être comprise entre 1990 et 2025.")]
        public int Year { get; set; }

        [Required]
        public string Brand { get; set; }

        [Required]
        public string Model { get; set; }
        [Display(Name = "Finition")]
        public string Trim { get; set; } // Finition

        [Column(TypeName = "decimal(10,2)")]
        public double Price { get; set; }

        public int? Km { get; set; }

        [Display(Name = "Date de mise en vente")]
        [DataType(DataType.Date)]
        public DateTime ToSellDate { get; set; }

        public string? ImageURL { get; set; }

        public ICollection<Repairing> Repairing { get; set; }
        public ICollection<Transaction> Transactions { get; set; }
    }
}

