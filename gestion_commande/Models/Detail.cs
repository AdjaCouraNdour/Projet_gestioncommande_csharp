
using System.ComponentModel.DataAnnotations;
using gestion_commande.Enums;
using gestion_commande.Validator;

namespace gestion_commande.Models
{
    public class Detail:AbstractEntity
    {
        public int Id { get; set; }
        public Detail()
        {
            CreateAt = DateTime.Now;
            UpdateAt = DateTime.Now;
        }

        public double QteCommande { get ; set; }
        public int CommandeId { get ; set; }  
        public Commande Commande { get ; set ; }
        public int ProduitId { get ; set; }  
        public Produit Produit { get ; set; }
        public static int Nbr { get ; set ; }
    }
}