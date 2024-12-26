using gestion_commande.Models;
using gestion_commande.Services.Interfaces;
using gestion_commande.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using gestion_commande.Enums;
using System.Security.Claims;

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
        public async Task<IActionResult> Index(int page = 1, int pageSize = 3)
        {
            var commandes = await _commandeService.GetCommandesByPaginate(page, pageSize);
            return View(commandes);
        }
        
        public async Task<IActionResult> CommandesClient(int page = 1, int pageSize = 3)
        {
            var userLogin = User.FindFirstValue(ClaimTypes.Name);
            if (userLogin == null)
            {
                return RedirectToAction("Login", "Account");
            }

            var client = await _clientService.FindClientByUserLogin(userLogin);
            if (client == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var commandes = await _commandeService.GetCommandesClientByPaginate(client.Id, page, pageSize);
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

        public IActionResult FormCommande()
        {
            var produits = _produitService.GetProduits();
            if (produits == null || !produits.Any())
            {
                ViewBag.MessageErreur = "Aucun produit disponible pour le moment.";
                produits = new List<Produit>();
            }

            ViewBag.Produits = produits;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FormCommande(List<ProduitCommande> produitsCommandes)
        {
            if (produitsCommandes == null || !produitsCommandes.Any())
            {
                ModelState.AddModelError("", "Aucun produit n'a été sélectionné.");
                return View();
            }

            foreach (var produitCommande in produitsCommandes)
            {
                // Vérification de la quantité
                if (produitCommande.Quantity <= 0)
                {
                    ModelState.AddModelError("", $"La quantité du produit {produitCommande.Produit.Libelle} doit être supérieure à zéro.");
                }

                // Vérification du stock
                var produit = await _produitService.FindById(produitCommande.ProduitId);
                if (produit == null || produit.QteStock < produitCommande.Quantity)
                {
                    ModelState.AddModelError("", $"La quantité demandée pour {produit.Libelle} dépasse le stock disponible.");
                }
            }

            if (!ModelState.IsValid)
            {
                return View();
            }

            var userLogin = User.FindFirstValue(ClaimTypes.Name);
            if (userLogin == null)
            {
                ModelState.AddModelError("", "Utilisateur non trouvé.");
                return View();
            }

            var client = await _clientService.FindClientByUserLogin(userLogin);
            if (client == null)
            {
                ModelState.AddModelError("", "Client introuvable.");
                return View();
            }

            var commande = new Commande
            {
                ClientId = client.Id,
                Client = client,
                ProduitsCommande = produitsCommandes,
                Montant = produitsCommandes.Sum(pc => pc.Quantity * pc.PrixUnitaire),
                MontantRestant = produitsCommandes.Sum(pc => pc.Quantity * pc.PrixUnitaire)
            };

            await _commandeService.Create(commande);
            TempData["Message"] = "Commande créée avec succès!";
            return RedirectToAction("Index");
        }
    }
}
