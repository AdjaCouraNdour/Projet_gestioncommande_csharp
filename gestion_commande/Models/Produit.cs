
using System.ComponentModel.DataAnnotations;
using gestion_commande.Enums;
using gestion_commande.Validator;

namespace gestion_commande.Models
{
    public class Produit:AbstractEntity
    {
        public int Id { get; set; }

       public Produit()
        {
            CreateAt = DateTime.Now;
            UpdateAt = DateTime.Now;
        }
       
        [Required(ErrorMessage = "Le libelle est obligatoire")]
        [UniqueLibelle(ErrorMessage = "Ce libelle est deja existant.")]
        public string Libelle { get ; set ; }
        
        [Required(ErrorMessage = "Le prix est obligatoire")]
        public int Prix { get ; set ; }

        [Required(ErrorMessage = "La qte est obligatoire")]
        public double QteStock { get ; set ; }

        public EtatProduit EtatProduit { get ; set ; }
        public virtual ICollection<Detail> Details { get;} = new List<Detail>();
        public virtual ICollection<ProduitCommande> ProduitsCommande { get;} = new List<ProduitCommande>();

    }
}