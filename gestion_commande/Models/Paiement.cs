
using System.ComponentModel.DataAnnotations;
using gestion_commande.Enums;
using gestion_commande.Validator;

namespace gestion_commande.Models
{
    public class Paiement:AbstractEntity
    {
        public int Id { get; set; } // Clé primaire
        public DateTime Date { get ; set ; }
        public int CommandeId { get ; set; }  
        public Commande Commande { get ; set ; }
        public TypePaiement TypePaiement { get; set; }

        [Required(ErrorMessage = "Le montant est obligatoire")]
        public double Montant { get ; set ; }
        public Paiement()
        {
            CreateAt = DateTime.Now;
            UpdateAt = DateTime.Now;
        }
    }
}