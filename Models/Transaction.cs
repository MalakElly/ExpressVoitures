
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

        //Un prix a display pour l'annonce de la voiture ()
        public decimal? SellingPrice { get; set; }

        //Un prix final de vente de la voiture 

        [Required(ErrorMessage ="Le prix d'achat de la voiture est obligatoire.")]
        public decimal BuyingPrice { get; set; }
        
        public DateTime BuyingDate { get; set; }

        [Display(Name = "Date de mise en vente")]
        [DataType(DataType.Date)]

        public DateTime? SellingDate { get; set; }

        public bool IsAvailable { get; set; }

        public int CarID { get; set; }

        [ForeignKey(nameof(CarID))]
        public Car Car { get; set; }

        public string? UserID { get; set; }
        [ValidateNever]
        
        [ForeignKey(nameof(UserID))]
        public User User { get; set; }

       
    }
}
