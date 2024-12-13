using gestion_commande.Core;
using gestion_commande.Data;
using gestion_commande.Services;
using gestion_commande.Enums;
using gestion_commande.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using gestion_commande.Models;


namespace gestion_commande.Services
{
    public class ClientService : IClientService
    {
        private readonly ApplicationDbContext _context;

        // Injecter le contexte de la base de données dans le service
        public ClientService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public IEnumerable<Client> GetClients()
        {
            return _context.Clients.ToList();
        }
        // Implémentation de la méthode Delete
        public async Task Delete(int id)
        {
            var client = await _context.Clients.FindAsync(id);
            if (client != null)
            {
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
            }
        }

        // Implémentation de la méthode FindAll
        public async Task<List<Client>> FindAll()
        {
            return await _context.Clients.ToListAsync();
        }

        // Implémentation de la méthode FindById
        public async Task<Client> FindById(int id)
        {
            return await _context.Clients.FindAsync(id);
        }

        // Implémentation de la méthode FindByTelephone
        public async Task<Client> FindByTelephone(string telephone)
        {
            return await _context.Clients
                .FirstOrDefaultAsync(c => c.Telephone == telephone);
        }

        // Implémentation de la méthode Save
        public async Task Save(Client data)
        {
            await _context.Clients.AddAsync(data);
            await _context.SaveChangesAsync();
        }

        // Implémentation de la méthode Update
        public async Task Update(Client data)
        {
            var existingClient = await _context.Clients.FindAsync(data.Id);
            if (existingClient != null)
            {
                _context.Entry(existingClient).CurrentValues.SetValues(data);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<Client> Create(Client client)
        {
            _context.Clients.Add(client);
            await _context.SaveChangesAsync();
            return client;
        }

        public async Task<Client> FindByUsernameAndTelephone(string username, string telephone)
        {
            return await _context.Clients
                         .FirstOrDefaultAsync(c => c.Username == username && c.Telephone == telephone);   
        }


    public async Task<PaginationModel<Client>> GetClientsByPaginate(int page, int pageSize)
        {
            var clients = _context.Clients.AsQueryable<Client>();
            return await PaginationModel<Client>.Paginate(clients, pageSize, page);

        }

    
    }
}
