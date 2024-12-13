using gestion_commande.Core;
using gestion_commande.Data;
using gestion_commande.Models;
using gestion_commande.Enums;
using gestion_commande.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace gestion_commande.Services
{
    public class PaiementService : IPaiementService
    {
        private readonly ApplicationDbContext _context;

        // Injecter le contexte de la base de données dans le service
        public PaiementService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public IEnumerable<Paiement> GetPaiements()
        {
            return _context.Paiements.ToList();
        }
        // Implémentation de la méthode Delete
        public async Task Delete(int id)
        {
            var paiement = await _context.Paiements.FindAsync(id);
            if (paiement != null)
            {
                _context.Paiements.Remove(paiement);
                await _context.SaveChangesAsync();
            }
        }

        // Implémentation de la méthode FindAll
        public async Task<List<Paiement>> FindAll()
        {
            return await _context.Paiements.ToListAsync();
        }

        // Implémentation de la méthode FindById
        public async Task<Paiement> FindById(int id)
        {
            return await _context.Paiements.FindAsync(id);
        }

        // Implémentation de la méthode Save
        public async Task Save(Paiement data)
        {
            await _context.Paiements.AddAsync(data);
            await _context.SaveChangesAsync();
        }

        // Implémentation de la méthode Update
        public async Task Update(Paiement data)
        {
            var existingPaiement = await _context.Paiements.FindAsync(data.Id);
            if (existingPaiement != null)
            {
                _context.Entry(existingPaiement).CurrentValues.SetValues(data);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<Paiement> Create(Paiement data)
        {
            _context.Paiements.Add(data);
            await _context.SaveChangesAsync();
            return data;
        }

        public async Task<IEnumerable<Paiement>> GetPaiementsCommande(int Id)
        {
            return await _context.Paiements.Where(p => p.CommandeId == Id).ToListAsync();
        }
        public async Task<PaginationModel<Paiement>> GetPaiementsByPaginate(int page, int pageSize)
        {
            var paiements = _context.Paiements.AsQueryable<Paiement>();
            return await PaginationModel<Paiement>.Paginate(paiements, pageSize, page);

        }
    
    }
}
