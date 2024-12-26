using gestion_commande.Core;
using gestion_commande.Models;
using System.Threading.Tasks;

namespace gestion_commande.Services.Interfaces
{
    public interface ICommandeService : IService<Commande>
    {
        IEnumerable<Commande> GetCommandes();
        Task<List<Commande>> FindByClientId(int ClientId);
        Task<Commande> Create(int clientId, Commande commande);
        Task<PaginationModel<Commande>> GetCommandesClientByPaginate(int clientId ,int page, int pageSize);     
        void CreerCommandePourClient(int clientId);
        Task<PaginationModel<Commande>> GetCommandesByPaginate(int page, int pageSize);
        Task<Commande> FindDetailsComdById(int id);
        Task ValiderCommande(Commande commande);
        Task MettreEnAttente(Commande commande);
    }
}
