using gestion_commande.Models;
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

        public ClientController(IClientService clientService, ICommandeService commandeService)
        {
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

        // Action pour ajouter un client
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FormClient([Bind("Surnom,Telephone,Address")] Client client)
        {
            if (ModelState.IsValid)
            {
                var clientAdded = await _clientService.Create(client);
                if (clientAdded != null)
                {
                    TempData["Message"] = "Client créé avec succès!";
                    return RedirectToAction(nameof(Index));  
                }
              
            }
            return View(client);
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

        [HttpGet]
        public IActionResult FormClientVal()
        {
            return View();
        }
        // Action pour valider les informations d'un client
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FormClientVal(string Username, string Telephone)
        {
            if (ModelState.IsValid)
            {
                var client = await _clientService.FindByUsernameAndTelephone(Username, Telephone);
                if (client != null)
                {
                    return RedirectToAction("FormCommande", new { clientId = client.Id });
                }
                else
                {
                    ModelState.AddModelError("", "Client non trouvé. Veuillez vérifier les informations.");
                }
            }
            return View();
        }
    }
}
