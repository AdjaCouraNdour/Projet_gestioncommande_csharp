using System.ComponentModel.DataAnnotations;
using gestion_commande.Enums;
using gestion_commande.Validator;

namespace gestion_commande.Models
{
    public class Client:AbstractEntity
    {
       public Client()
        {
            CreateAt = DateTime.Now;
            UpdateAt = DateTime.Now;
        }

        public int Id { get; set; }
        public User? User { get; set; }
        public int? UserId { get; set; }

        public virtual ICollection<Commande>? Commandes { get;} = new List<Commande>();

    }
}
