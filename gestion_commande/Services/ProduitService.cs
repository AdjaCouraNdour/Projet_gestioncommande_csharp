using gestion_commande.Core;
using gestion_commande.Data;
using gestion_commande.Models;
using gestion_commande.Enums;
using gestion_commande.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace gestion_commande.Services
{
    public class ProduitService : IProduitService
    {
        private readonly ApplicationDbContext _context;

        // Injecter le contexte de la base de données dans le service
        public ProduitService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public  IEnumerable<Produit> GetProduits()
        {
            return _context.Produits.ToList();
        }
        // Implémentation de la méthode Delete
        public async Task Delete(int id)
        {
            var produit = await _context.Produits.FindAsync(id);
            if (produit != null)
            {
                _context.Produits.Remove(produit);
                await _context.SaveChangesAsync();
            }
        }

        // Implémentation de la méthode FindAll
        public async Task<List<Produit>> FindAll()
        {
            return await _context.Produits.ToListAsync();
        }

        // Implémentation de la méthode FindById
        public async Task<Produit> FindById(int id)
        {
            return await _context.Produits.FindAsync(id);
        }

        // Implémentation de la méthode Save
         public async Task Save(Produit data)
        {
            await _context.Produits.AddAsync(data);
            await _context.SaveChangesAsync();
        }

        // Implémentation de la méthode Update
        public async Task Update(Produit data)
        {
            var existingProduit = await _context.Produits.FindAsync(data.Id);
            if (existingProduit != null)
            {
                _context.Entry(existingProduit).CurrentValues.SetValues(data);
                await _context.SaveChangesAsync();
            }
        }

        public Task<Produit> FindByEtat(EtatProduit etat)
        {
            throw new NotImplementedException();
        }

        public async Task<Produit> Create(Produit data)
         {

            _context.Produits.Add(data);
            await _context.SaveChangesAsync();
            return data;
        }
        public async Task<PaginationModel<Produit>> GetProduitsByPaginate(int page, int pageSize)
        {
            var produits = _context.Produits.AsQueryable<Produit>();
            return await PaginationModel<Produit>.Paginate(produits, pageSize, page);

        }

    }
}
