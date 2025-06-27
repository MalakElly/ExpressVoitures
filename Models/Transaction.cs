using GestionVoituresExpress.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace GestionVoituresExpress.Models
{
    public class Transaction
    {
        [Key]
        public int TransactionID { get; set; }

        [Display(Name = "Prix de vente")]
        [Range(0, 1_000_000, ErrorMessage = "Le prix de vente doit être un montant positif.")]
        public decimal? SellingPrice { get; set; }

        [Required(ErrorMessage = "Le prix d'achat de la voiture est obligatoire.")]
        [Range(10, 1_000_000, ErrorMessage = "Le prix d'achat doit être positif.")]
        [Display(Name = "Prix d'achat")]
        public decimal BuyingPrice { get; set; }

        [Required(ErrorMessage = "La date d'achat est obligatoire.")]
        [DataType(DataType.Date)]
    
        [Display(Name = "Date d'achat")]
        public DateTime BuyingDate { get; set; }

        [Display(Name = "Date de mise en vente")]
        [DataType(DataType.Date)]
        public DateTime? SellingDate { get; set; }

        [Display(Name = "Disponible à la vente")]
        public bool IsAvailable { get; set; }

        public int CarID { get; set; }

        [ForeignKey(nameof(CarID))]
        [ValidateNever] 
        public Car? Car { get; set; }

        public string? UserID { get; set; }

        [ForeignKey(nameof(UserID))]
        [ValidateNever]
        public User? User { get; set; }
    }
}
