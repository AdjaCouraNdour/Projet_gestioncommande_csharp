using gestion_commande.Core;
using gestion_commande.Data;
using gestion_commande.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using gestion_commande.Models;


namespace gestion_commande.Services
{
    public class CommandeService : ICommandeService
    {
        private readonly ApplicationDbContext _context;

        // Injecter le contexte de la base de données dans le service
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
            // Récupérer le client pour valider qu'il existe
            var client = await _context.Clients.FindAsync(clientId);
            if (client == null)
            {
                // Si le client n'existe pas, vous pouvez gérer l'erreur ici (par exemple, lever une exception ou retourner null)
                throw new Exception("Client non trouvé.");
            }

            // Associer la Commande au client
            Commande.ClientId = clientId;
            Commande.Client = client; // Si la relation entre Commande et Client est définie

            // Ajouter la Commande au contexte
            _context.Commandes.Add(Commande);

            // Sauvegarder les modifications dans la base de données
            await _context.SaveChangesAsync();

            return Commande; // Retourner la Commande créée
        }

        public Task<Commande> Create(Commande data)
        {
            throw new NotImplementedException();
        }
        public async Task<PaginationCommandeModel> GetCommandesClientByPaginate(int clientId, int page, int pageSize)
        {
            var commandes = _context.Commandes.Where(commande => commande.ClientId == clientId).AsQueryable<Commande>();
            var client = await _context.Clients.SingleAsync<Client>(client => client.Id == clientId)!;
            return await PaginationCommandeModel.PaginateCommande(commandes, pageSize, page, client);
        }
    }
}
