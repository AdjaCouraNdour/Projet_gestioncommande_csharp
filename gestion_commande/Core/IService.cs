using System.Collections.Generic;
using System.Threading.Tasks;

namespace gestion_commande.Core
{
    public interface IService<T>
    {
        // Récupérer tous les éléments
        Task<List<T>> FindAll();

        // Récupérer un élément par son ID
        Task<T> FindById(int id);

        // Sauvegarder un élément
        Task<T> Create(T data);

        // Supprimer un élément par son ID
        Task Delete(int id);

        // Mettre à jour un élément
        Task Update(T data);
    }
}
