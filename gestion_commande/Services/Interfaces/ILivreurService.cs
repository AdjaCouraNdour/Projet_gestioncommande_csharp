using gestion_commande.Core;
using gestion_commande.Models;
using gestion_commande.Enums;


namespace gestion_commande.Services.Interfaces
{
    public interface ILivreurService : IService<Livreur>
    {
        Task<Livreur> FindByEtat(EtatLivreur etat);
        IEnumerable<Livreur> GetLivreurs();
        Task<IEnumerable<Livreur>> GetLivreursDispo();
        Task<PaginationModel<Livreur>> GetLivreursByPaginate(int page, int pageSize);

    }
}
