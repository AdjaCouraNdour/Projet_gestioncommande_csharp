using gestion_commande.Core;
using gestion_commande.Data;
using gestion_commande.Models;
using gestion_commande.Enums;
using gestion_commande.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace gestion_commande.Services
{
    public class DetailsService : IDetailsService
    {
        private readonly ApplicationDbContext _context;

        // Injecter le contexte de la base de données dans le service
        public DetailsService(ApplicationDbContext context)
        {
            _context = context;
        }
        

        public IEnumerable<Detail> GetDetails()
        {
            return _context.Details.ToList();
        }
        // Implémentation de la méthode Delete
        public async Task Delete(int id)
        {
            var detail = await _context.Details.FindAsync(id);
            if (detail != null)
            {
                _context.Details.Remove(detail);
                await _context.SaveChangesAsync();
            }
        }

        // Implémentation de la méthode FindAll
        public async Task<List<Detail>> FindAll()
        {
            return await _context.Details.ToListAsync();
        }

        // Implémentation de la méthode FindById
        public async Task<Detail> FindById(int id)
        {
            return await _context.Details.FindAsync(id);
        }

        // Implémentation de la méthode Save
        public async Task Save(Detail data)
        {
            await _context.Details.AddAsync(data);
            await _context.SaveChangesAsync();
        }

        // Implémentation de la méthode Update
        public async Task Update(Detail data)
        {
            var existingDetail = await _context.Details.FindAsync(data.Id);
            if (existingDetail != null)
            {
                _context.Entry(existingDetail).CurrentValues.SetValues(data);
                await _context.SaveChangesAsync();
            }
        }
      public async Task<List<Detail>> FindProduitByCommandeId(int commandeId)
        {
            return await _context.Details
                .Where(de => de.CommandeId == commandeId)
                .Include(de => de.Produit) // Inclut les informations de l'Produit si nécessaire
                .ToListAsync();
        }

        public async Task<Detail> Create(Detail data)
        {
            throw new NotImplementedException();
        }
    }
}
