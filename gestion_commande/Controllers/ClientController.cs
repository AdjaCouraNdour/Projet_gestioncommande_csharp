using gestion_commande.Models;
using gestion_commande.Data;
using gestion_commande.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace gestion_commande.Controllers
{
    public class ClientController : Controller
    {
        private readonly IClientService _clientService;
        private readonly ICommandeService _commandeService;
        private readonly ApplicationDbContext _context;


        public ClientController(ApplicationDbContext context,IClientService clientService, ICommandeService commandeService)
        {
            _context = context;
            _clientService = clientService;
            _commandeService = commandeService;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 3)
        {
            // Fetch clients from the service
            var clients = await _clientService.GetClientsByPaginate(page, pageSize);
            
            // Pass the clients to the view
            return View(clients);
        }
        
        // Action pour afficher le formulaire de création d'un client
        [HttpGet]
        public IActionResult FormClient()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> FormClient(User user)
        {
            if (ModelState.IsValid)
            {
                // Logique pour créer l'utilisateur et le client
                var client = new Client();
                user.Client = client;

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return RedirectToAction("SuccessPage");
            }

            // En cas d'erreur, retourner à la même vue avec les messages de validation
            return View(user);
        }

        // Action pour afficher les détails d'un client
        public async Task<IActionResult> DetailsClient(int id)
        {
            var client = await _clientService.FindById(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // Action pour afficher le formulaire d'édition d'un client
        public async Task<IActionResult> Edit(int id)
        {
            var client = await _clientService.FindById(id);
            if (client == null)
            {
                return NotFound();
            }
            return View(client);
        }

        // Action pour mettre à jour un client
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Client client)
        {
            if (id != client.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _clientService.Update(client);
                return RedirectToAction(nameof(Index));
            }
            return View(client);
        }

        // Action pour supprimer un client
        public async Task<IActionResult> Delete(int id)
        {
            var client = await _clientService.FindById(id);
            if (client == null)
            {
                return NotFound();
            }

            await _clientService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

    }
}
