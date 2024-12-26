
using System.ComponentModel.DataAnnotations;
using gestion_commande.Enums;
using gestion_commande.Validator;

namespace gestion_commande.Models
{
    public class Livreur:AbstractEntity
    {
         public int Id { get; set; }

       public Livreur()
        {
            CreateAt = DateTime.Now;
            UpdateAt = DateTime.Now;
        }
        [UniqueNomComplet(ErrorMessage="Ce nom est deja existant")]
        [Required(ErrorMessage = "nomComplet est obligatoire")]
        [StringLength(50, ErrorMessage = "L'nomComplet doit avoir au maximum 20 caractères")]
        public string NomComplet { get; set; } = string.Empty;

        [UniqueEmail(ErrorMessage="Cet email est deja existant")]
        [Required(ErrorMessage = "L'email est obligatoire")]
        [StringLength(50, ErrorMessage = "L'email doit avoir au maximum 20 caractères")]
        public string Email { get; set; } = string.Empty;

        [UniqueTelephone(ErrorMessage="le telephone doit etre unique")]
        [Required(ErrorMessage = "Le telephone est obligatoire")]
        [RegularExpression(@"^(77|78|76)[0-9]{7}$",ErrorMessage = "Le telephone doit etrre sous forme 77xxxxxxx ou 78xxxxxxx ou 76xxxxxxx")]
        public string Telephone { get; set; }
        public EtatLivreur EtatLivreur { get; set; }

    }
}