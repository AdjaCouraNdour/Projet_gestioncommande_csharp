using gestion_commande.Core;
using gestion_commande.Models;
using System.Threading.Tasks;

namespace gestion_commande.Services.Interfaces
{
    public interface IClientService : IService<Client>
    {
        Task<Client> FindByTelephone(string telephone);
        IEnumerable<Client> GetClients();
        Task<Client> FindByUsernameAndTelephone(string username, string telephone);
        Task<PaginationModel<Client>> GetClientsByPaginate(int page, int pageSize);

    }
}
