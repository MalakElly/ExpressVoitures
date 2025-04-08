using System.ComponentModel.DataAnnotations;

namespace GestionVoituresExpress.Models
{
    public class RepairingAndType
    {
        
        public int RepairingId { get; set; }
        public Repairing Repairing { get; set; }

        public int RepairingTypeId { get; set; }
        public RepairingType RepairingType { get; set; }
    }
}
