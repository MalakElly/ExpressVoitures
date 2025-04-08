using Microsoft.AspNetCore.Identity;



namespace GestionVoituresExpress.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Address { get; set; } 
        public bool IsAdmin { get; set; } = false;
        public ICollection<Transaction> Transactions { get; set; }
    }
}


//Mettre une page pour ajouter Admin (Interface  pour créer des utilisateurs ou admins)
//Condtionner qui peut gérer cette interface (uniquement un admin)
//Partial Interface 