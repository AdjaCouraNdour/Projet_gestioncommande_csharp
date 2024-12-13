using gestion_commande.Models;
using gestion_commande.Services.Interfaces;
using gestion_commande.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using gestion_commande.Enums;

namespace gestion_commande.Controllers
{
    public class commandeController : Controller
    {
        private readonly ICommandeService _commandeService;
        private readonly IDetailsService _detailsService;
        private readonly IPaiementService _paiementService;
        private readonly IClientService _clientService;
        private readonly IProduitService _produitService;

        public commandeController(ICommandeService commandeService, IDetailsService detailsService, IPaiementService paiementService, IClientService clientService,IProduitService produitService)
        {
            _commandeService = commandeService;
            _detailsService = detailsService;
            _paiementService = paiementService;
            _clientService = clientService;
            _produitService=produitService;
        }

        // Action pour afficher la liste des commandes avec pagination
        public IActionResult Index(int page = 1, int limit = 3)
        {
            var commandes = _commandeService.GetCommandes()
                          .OrderBy(c => c.Id)
                          .Skip((page - 1) * limit)
                          .Take(limit)
                          .ToList();

            int totalCommandes = commandes.Count();
            var commandesPaginated = commandes.Skip((page - 1) * limit).Take(limit).ToList();

            ViewBag.Page = page;
            ViewBag.limit = limit;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalCommandes / limit);

            return View(commandesPaginated);
        }
         public async Task<IActionResult> CommandesClient(int clientId, int page = 1, int pageSize = 3)
        {
            var commandes = await _commandeService.GetCommandesClientByPaginate(clientId, page, pageSize);
            return View(commandes);
        }
        // Action pour afficher les détails d'une commande
        public async Task<IActionResult> Details(int id)
        {
            var commande = await _commandeService.FindById(id);
            if (commande == null)
            {
                return NotFound();
            }
            return View(commande);
        }

        // Action pour afficher le formulaire de création d'une commande
        public async Task<IActionResult> FormCommande(int clientId)
        {
            var client = await _clientService.FindById(clientId);
            if (client == null)
            {
                return NotFound();
            }

            // Récupérer la liste des produits disponibles
            var produits = _produitService.GetProduits();
            ViewBag.produits = produits;

            ViewBag.Client = client;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FormCommande(int clientId, List<ProduitSelection> produitSelections)
        {
            if (ModelState.IsValid)
            {
                // Trouver le client associé
                var client = await _clientService.FindById(clientId);
                if (client == null)
                {
                    return NotFound();
                }

                // Créer une nouvelle commande
                var commande = new Commande
                {
                    ClientId = clientId,
                    Details = new List<Detail>() // Initialisation de la liste des détails
                };

                // Ajouter chaque produit sélectionné à la commande en créant des détails
                foreach (var selection in produitSelections)
                {
                    var produit = await _produitService.FindById(selection.ProduitId); // Find the produit
                    if (produit != null)
                    {
                        var detail = new Detail
                        {
                            ProduitId = selection.ProduitId,
                            QteCommande = selection.Quantity,
                            Commande = commande // Link the detail to the debt
                        };

                        commande.Details.Add(detail); // Add the detail to the debt's details list
                        commande.Montant += selection.Quantity * produit.Prix; // Add to the total debt amount
                    }
                }


                // Calculer le montant total de la commande

                // Sauvegarder la commande et ses détails
                await _commandeService.Create(clientId, commande);
                TempData["Message"] = "commande créée avec succès!";
                return RedirectToAction("Index", "Client");
            }

            return View();
        }

// Classe pour lier les produits et la quantité
        public class ProduitSelection
        {
            public int ProduitId { get; set; }
            public int Quantity { get; set; }
        }

        // Action pour éditer une commande
        public async Task<IActionResult> Edit(int id)
        {
            var commande = await _commandeService.FindById(id);
            if (commande == null)
            {
                return NotFound();
            }
            return View(commande);
        }

        // Action pour mettre à jour une commande
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Commande commande)
        {
            if (id != commande.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _commandeService.Update(commande);
                return RedirectToAction(nameof(Index));
            }
            return View(commande);
        }

        [HttpPost]
        public async Task<IActionResult> Accepte(int commandeId)
        {
            // Rechercher la commande par ID
            var commande = await _commandeService.FindById(commandeId);
            if (commande == null)
            {
                return NotFound();
            }

            // Mettre à jour l'état de la commande à "acceptée"
            commande.EtatCommande = EtatCommande.Valide;
            _commandeService.Update(commande);

            // Rediriger ou retourner une vue
            return RedirectToAction("Index"); // Remplacez "Index" par la page que vous souhaitez afficher.
        }

        [HttpPost]
        public async Task<IActionResult> Refuse(int commandeId)
        {
            // Rechercher la commande par ID
            var commande = await _commandeService.FindById(commandeId);
            if (commande == null)
            {
                return NotFound();
            }

            // Mettre à jour l'état de la commande à "annulée"
            commande.EtatCommande = EtatCommande.EnAttente;
            _commandeService.Update(commande);

            // Rediriger ou retourner une vue
            return RedirectToAction("Index"); // Remplacez "Index" par la page que vous souhaitez afficher.
        }

        // Action pour supprimer une commande
        public async Task<IActionResult> Delete(int id)
        {
            var commande = await _commandeService.FindById(id);
            if (commande == null)
            {
                return NotFound();
            }
            return View(commande);
        }

        // Action pour confirmer la suppression d'une commande
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _commandeService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

      
    }

}
