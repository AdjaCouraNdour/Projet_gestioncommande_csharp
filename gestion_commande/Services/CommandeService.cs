using gestion_commande.Core;
using gestion_commande.Data;
using gestion_commande.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using gestion_commande.Models;
using gestion_commande.Enums;


namespace gestion_commande.Services
{
    public class CommandeService : ICommandeService
    {
        private readonly ApplicationDbContext _context;

        public CommandeService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public IEnumerable<Commande> GetCommandes()
        {
            return _context.Commandes.ToList();
        }
        // Implémentation de la méthode Delete
        public async Task Delete(int id)
        {
            var commande = await _context.Commandes.FindAsync(id);
            if (commande != null)
            {
                _context.Commandes.Remove(commande);
                await _context.SaveChangesAsync();
            }
        }

        // Implémentation de la méthode FindAll
        public async Task<List<Commande>> FindAll()
        {
            return await _context.Commandes.ToListAsync();
        }

        // Implémentation de la méthode FindById
        public async Task<Commande> FindById(int id)
        {
            return await _context.Commandes.FindAsync(id);
        }
        public async Task<Commande> FindDetailsComdById(int id)
        {
            return await _context.Commandes
                .Include(c => c.ProduitsCommande)
                .ThenInclude(pc => pc.Produit) 
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        // Implémentation de la méthode Save
        public async Task Save(Commande data)
        {
            await _context.Commandes.AddAsync(data);
            await _context.SaveChangesAsync();
        }

        // Implémentation de la méthode Update
        public async Task Update(Commande data)
        {
            var existingCommande = await _context.Commandes.FindAsync(data.Id);
            if (existingCommande != null)
            {
                _context.Entry(existingCommande).CurrentValues.SetValues(data);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Commande>?> FindByClientId(int client)
        {
            return await _context.Commandes
                .Where(d => d.ClientId == client) 
                .ToListAsync();
        }

       public async Task<Commande> Create(int clientId, Commande Commande)
        {
            var client = await _context.Clients.FindAsync(clientId);
            if (client == null)
            {
                throw new Exception("Client non trouvé.");
            }
            Commande.ClientId = clientId;
            Commande.Client = client; 
            _context.Commandes.Add(Commande);
            await _context.SaveChangesAsync();

            return Commande; 
        }

  
         public async Task<Commande> Create(Commande commande)
        {
            // Ajouter la commande dans le contexte
            await _context.Commandes.AddAsync(commande);

            // Sauvegarder les changements
            await _context.SaveChangesAsync();

            return commande;
        }

         public async Task<PaginationModel<Commande>> GetCommandesByPaginate(int page, int pageSize)
        {
            var commandes = _context.Commandes
                .Include(d => d.Client)  // Inclure le Client
                .ThenInclude(c => c.User)  // Inclure User à l'intérieur du Client
                .AsQueryable();

            return await PaginationModel<Commande>.Paginate(commandes, pageSize, page);
        }
        public async Task<PaginationModel<Commande>> GetCommandesClientByPaginate(int clientId, int page, int pageSize)
        {
            var commandes = _context.Commandes.Where(commande => commande.ClientId == clientId).AsQueryable();
            var client = await _context.Clients.SingleOrDefaultAsync(client => client.Id == clientId);
            return await PaginationCommandeModel.PaginateCommande(commandes, pageSize, page, client);
        }

        public void CreerCommandePourClient(int clientId)
        {
            throw new NotImplementedException();
        }

        public async Task CommandeDeclarerRecu(Commande commande)
        {
            if (commande == null) throw new ArgumentNullException(nameof(commande));

            commande.EtatCommande = EtatCommande.Recu;
            _context.Commandes.Update(commande);
            await _context.SaveChangesAsync();
        }
        
        public async Task ValiderCommande(Commande commande)
        {
            if (commande == null) throw new ArgumentNullException(nameof(commande));

            commande.EtatCommande = EtatCommande.Valide;
            _context.Commandes.Update(commande);
            await _context.SaveChangesAsync();
        }
        public async Task MettreEnAttente(Commande commande)
        {
            if (commande == null) throw new ArgumentNullException(nameof(commande));
            commande.EtatCommande = EtatCommande.EnAttente;
            _context.Commandes.Update(commande);
            await _context.SaveChangesAsync();
        }
      
    }
}

