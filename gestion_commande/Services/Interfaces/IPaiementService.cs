using gestion_commande.Core;
using gestion_commande.Models;


namespace gestion_commande.Services.Interfaces
{
    public interface IPaiementService : IService<Paiement>
    {
        IEnumerable<Paiement> GetPaiements();   
        Task<IEnumerable<Paiement>> GetPaiementsCommande(int Id);
        Task<PaginationModel<Paiement>> GetPaiementsByPaginate(int page, int pageSize);

 }
}
