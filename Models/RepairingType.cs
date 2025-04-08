using System.ComponentModel.DataAnnotations;

namespace GestionVoituresExpress.Models
{
    
    public class RepairingType
    {
        [Key]
        public int Id { get; set; }
        public string NomReparation { get; set; }
        public int PrixReparation { get; set; }
        public ICollection<Repairing> Repairings { get; set; }
        
    }
}
