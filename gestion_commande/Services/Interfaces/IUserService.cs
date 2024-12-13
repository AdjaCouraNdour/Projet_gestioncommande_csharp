using gestion_commande.Core;
using gestion_commande.Models;


namespace gestion_commande.Services.Interfaces
{
    public interface IUserService : IService<User>
    {
        Task<User> FindByLogin(string telephone);
        Task<User> FindByEmail(string telephone);
        IEnumerable<User> GetUsers();
        Task<PaginationModel<User>> GetUsersByPaginate(int page, int pageSize);

    }
}
