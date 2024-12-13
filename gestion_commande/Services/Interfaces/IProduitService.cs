using gestion_commande.Core;
using gestion_commande.Models;
using gestion_commande.Enums;


namespace gestion_commande.Services.Interfaces
{
    public interface IProduitService : IService<Produit>
    {
        Task<Produit> FindByEtat(EtatProduit etat);
        IEnumerable<Produit> GetProduits();
        Task<PaginationModel<Produit>> GetProduitsByPaginate(int page, int pageSize);

    }
}
