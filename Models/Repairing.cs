using Microsoft.CodeAnalysis.Operations;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionVoituresExpress.Models
{
    public class RepairingType
    {
        [Key]
        public int Id{  get; set; }
        public string NomReparation { get; set; }
        public int PrixReparation { get; set; }
        public ICollection<Repairing> Repairings { get; set; }7
        public int MyProperty { get; set; }
    }
    public class Repairing
    {
        [Key]
        public int RepairingId { get; set; }
        public string Name { get; set; }//Nom de la réparation
        public DateTime RepairDate { get; set; }
        public RepairingType RepairingType { get; set; }
        public double RepairingPrice { get; set; } //Prix de la reparation
        
       public int CarID { get; set; }

        [ForeignKey(nameof(CarID))]
        public Car Car { get; set; }
        public ICollection<RepairingType> RepairingTypes { get; set; }


    }
}
