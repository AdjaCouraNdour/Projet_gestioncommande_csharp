using gestion_commande.Models;


namespace gestion_commande.Models
{
    public class ProduitCommande
    {
        public int Id { get; set; } 
        public int Quantity { get; set; }  
        public int PrixUnitaire { get; set; }  
        public int ProduitId { get; set; }  
        public Produit Produit { get; set; } 
        public int CommandeId { get; set; }  
        public Commande Commande { get; set; } 

    }
}