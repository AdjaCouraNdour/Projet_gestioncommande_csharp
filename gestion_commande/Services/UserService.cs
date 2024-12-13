using gestion_commande.Core;
using gestion_commande.Data;
using gestion_commande.Models;
using gestion_commande.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace gestion_commande.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _context;

        // Injecter le contexte de la base de données dans le service
        public UserService(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public IEnumerable<User> GetUsers()
        {
            return _context.Users.ToList();
        }
        // Implémentation de la méthode Delete
        public async Task Delete(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        // Implémentation de la méthode FindAll
        public async Task<List<User>> FindAll()
        {
            return await _context.Users.ToListAsync();
        }

        // Implémentation de la méthode FindById
        public async Task<User> FindById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        // Implémentation de la méthode Save
        public async Task Save(User data)
        {
            await _context.Users.AddAsync(data);
            await _context.SaveChangesAsync();
        }

        // Implémentation de la méthode Update
        public async Task Update(User data)
        {
            var existingUser = await _context.Users.FindAsync(data.Id);
            if (existingUser != null)
            {
                _context.Entry(existingUser).CurrentValues.SetValues(data);
                await _context.SaveChangesAsync();
            }
        }

        public Task<User> FindByLogin(string telephone)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindByEmail(string telephone)
        {
            throw new NotImplementedException();
        }

        public async Task<User> Create(User data)
         {
            _context.Users.Add(data);
            await _context.SaveChangesAsync();
            return data;
        }
        public async Task<PaginationModel<User>> GetUsersByPaginate(int page, int pageSize)
        {
            var users = _context.Users.AsQueryable<User>();
            return await PaginationModel<User>.Paginate(users, pageSize, page);

        }
    
    }
}
