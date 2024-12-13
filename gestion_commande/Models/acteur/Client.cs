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
            UserRole= UserRole.Client;
        }

        public int Id { get; set; }

        [UniqueLoginUsername(ErrorMessage="Ce login/Username est deja existant")]
        [Required(ErrorMessage = "Le login/Username est obligatoire")]
        [StringLength(20, ErrorMessage = "Le  login/Username doit avoir au maximum 20 caractères")]
        public string Login { get; set; } = string.Empty;

        [UniqueEmail(ErrorMessage="Cet email est deja existant")]
        [Required(ErrorMessage = "L'email est obligatoire")]
        [StringLength(50, ErrorMessage = "L'email doit avoir au maximum 20 caractères")]
        public string Email { get; set; } = string.Empty;

        [UniqueTelephone(ErrorMessage="le telephone doit etre unique")]
        [Required(ErrorMessage = "Le telephone est obligatoire")]
        [RegularExpression(@"^(77|78|76)[0-9]{7}$",ErrorMessage = "Le telephone doit etrre sous forme 77xxxxxxx ou 78xxxxxxx ou 76xxxxxxx")]
        public string Telephone { get; set; }

        [Required(ErrorMessage = "Le password est obligatoire")]
        public string Password { get; set; } = string.Empty;
        public UserRole UserRole { get; set; }
        public string Address { get; set; }

        public virtual ICollection<Commande>? Commandes { get;} = new List<Commande>();

    }
}
