using gestion_commande.Core;
using gestion_commande.Data;
using gestion_commande.Models;
using gestion_commande.Enums;
using gestion_commande.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace gestion_commande.Services
{
    public class LivreurService : ILivreurService
    {
        private readonly ApplicationDbContext _context;

        // Injecter le contexte de la base de données dans le service
        public LivreurService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public  IEnumerable<Livreur> GetLivreurs()
        {
            return _context.Livreurs.ToList();
        }
        // Implémentation de la méthode Delete
        public async Task Delete(int id)
        {
            var Livreur = await _context.Livreurs.FindAsync(id);
            if (Livreur != null)
            {
                _context.Livreurs.Remove(Livreur);
                await _context.SaveChangesAsync();
            }
        }

        // Implémentation de la méthode FindAll
        public async Task<List<Livreur>> FindAll()
        {
            return await _context.Livreurs.ToListAsync();
        }

        // Implémentation de la méthode FindById
        public async Task<Livreur> FindById(int id)
        {
            return await _context.Livreurs.FindAsync(id);
        }

        // Implémentation de la méthode Save
         public async Task Save(Livreur data)
        {
            await _context.Livreurs.AddAsync(data);
            await _context.SaveChangesAsync();
        }

        // Implémentation de la méthode Update
        public async Task Update(Livreur data)
        {
            var existingLivreur = await _context.Livreurs.FindAsync(data.Id);
            if (existingLivreur != null)
            {
                _context.Entry(existingLivreur).CurrentValues.SetValues(data);
                await _context.SaveChangesAsync();
            }
        }

        public Task<Livreur> FindByEtat(EtatLivreur etat)
        {
            throw new NotImplementedException();
        }

        public async Task<Livreur> Create(Livreur data)
         {

            _context.Livreurs.Add(data);
            await _context.SaveChangesAsync();
            return data;
        }
        public async Task<PaginationModel<Livreur>> GetLivreursByPaginate(int page, int pageSize)
        {
            var Livreurs = _context.Livreurs.AsQueryable<Livreur>();
            return await PaginationModel<Livreur>.Paginate(Livreurs, pageSize, page);

        }

    }
}
