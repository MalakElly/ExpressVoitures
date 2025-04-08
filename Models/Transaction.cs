
using GestionVoituresExpress.Models;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GestionVoituresExpress.Models
{
    public enum TransactionType
    {
        Buying = 1,
        Selling = 2,
        Repairing = 3
    }

    public class Transaction
    {
        [Key]
        public int TransactionID { get; set; }
        public DateTime date { get; set; }
        public double SellingPrice { get; set; }
        public double BuyingPrice { get; set; }
        
     

        
        public int CarID { get; set; }
        [ForeignKey(nameof(CarID))]
        public Car Car { get; set; }

        public string? UserID { get; set; }

        [ForeignKey(nameof(UserID))]
        public User User { get; set; }
       
    }
}
