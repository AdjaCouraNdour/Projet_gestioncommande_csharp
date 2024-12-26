
using System.ComponentModel.DataAnnotations;
using gestion_commande.Enums;
using gestion_commande.Validator;

namespace gestion_commande.Models
{
    public class Commande:AbstractEntity
    {
      public int Id { get; set; }
        public DateTime Date { get; set; }
        public double Montant { get; set; }
        public double MontantVerse { get; set; }
        public double MontantRestant { get; set; }
        public Client Client { get; set; }
        public static int Nbr { get; set; }
        public int ClientId { get; set; }
        public Commande()
        {
            Date = DateTime.Now;
            CreateAt = DateTime.Now;
            UpdateAt = DateTime.Now;
        }
        public EtatCommande EtatCommande { get; set ; }
    
        public virtual ICollection<Detail> Details { get;set;} = new List<Detail>();
        public virtual ICollection<Paiement> Paiements { get;} = new List<Paiement>();
        public virtual ICollection<ProduitCommande> ProduitsCommande { get; set;} = new List<ProduitCommande>();


    }
}