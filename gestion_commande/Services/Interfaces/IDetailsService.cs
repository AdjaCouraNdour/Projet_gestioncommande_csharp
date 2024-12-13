using gestion_commande.Core;
using gestion_commande.Models;


namespace gestion_commande.Services.Interfaces
{
    public interface IDetailsService : IService<Detail>
    {   
        IEnumerable<Detail> GetDetails();
        Task<List<Detail>?> FindProduitByCommandeId(int commandeId);
    }
}
