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
        Task<PaginationCommandeModel> GetCommandesClientByPaginate(int clientId, int page, int pageSize);

    }
}
