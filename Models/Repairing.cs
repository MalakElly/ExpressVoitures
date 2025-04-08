using Microsoft.CodeAnalysis.Operations;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionVoituresExpress.Models
{
    
    public class Repairing
    {
        [Key]
        public int RepairingId { get; set; }
       
        public DateTime RepairingDate { get; set; }
       
        public double RepairingPrice { get; set; } //Prix de la reparation
        
       public int CarID { get; set; }

        [ForeignKey(nameof(CarID))]
        public Car Car { get; set; }
        public ICollection<RepairingAndType> RepairingAndTypes { get; set; }


    }
}
