using System.ComponentModel.DataAnnotations;

namespace GestionVoituresExpress.Models
{
    
    public class RepairingType
    {
        [Key]
        public int RepairingTypeId { get; set; }
        public string RepairingName { get; set; }

        public ICollection<RepairingAndType> RepairingAndTypes { get; set; }

    }
}
