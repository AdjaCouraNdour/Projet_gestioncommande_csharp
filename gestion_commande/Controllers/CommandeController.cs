using gestion_commande.Models;
using gestion_commande.Services.Interfaces;
using gestion_commande.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using gestion_commande.Enums;
using System.Security.Claims;
using gestion_commande.Data;

namespace gestion_commande.Controllers
{
    public class commandeController : Controller
    {
        private readonly ICommandeService _commandeService;
        private readonly IPaiementService _paiementService;
        private readonly IClientService _clientService;
        private readonly ILivreurService _livreurService;
        private readonly IProduitService _produitService;

        public commandeController(ICommandeService commandeService, IPaiementService paiementService, IClientService clientService,ILivreurService livreurService,IProduitService produitService)
        {
            _commandeService = commandeService;
            _paiementService = paiementService;
            _clientService = clientService;
            _livreurService = livreurService;
            _produitService=produitService;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 3)
        {
            var commandes = await _commandeService.GetCommandesByPaginate(page, pageSize);
            return View(commandes);
        }
        
        public async Task<IActionResult> CommandesClient(int clientId ,int page = 1, int pageSize = 3)
        {
            var client = await _clientService.FindById(clientId);
            if (client == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var commandes = await _commandeService.GetCommandesClientByPaginate(client.Id, page, pageSize);
            return View(commandes);
        }
        
        public async Task<IActionResult> ListeDeMesCommandes(int page = 1, int pageSize = 3)
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
       public async Task<IActionResult> DetailsCommande(int id)
        {
            var commande = await _commandeService.FindDetailsComdById(id);
            if (commande == null)
            {
                return NotFound();
            }
            var produitsCommande = commande.ProduitsCommande; 
            return View(produitsCommande);
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
                EtatCommande = EtatCommande.Encours,
                ProduitsCommande = produitsCommandes,
                Montant = produitsCommandes.Sum(pc => pc.Quantity * pc.PrixUnitaire),
                MontantRestant = produitsCommandes.Sum(pc => pc.Quantity * pc.PrixUnitaire)
            };

            await _commandeService.Create(commande);
            TempData["Message"] = "Commande créée avec succès!";
            return RedirectToAction("Index");
        }


        [HttpPost]
        public async Task<IActionResult> DeclarerRecu(int idCommande)
        {
            // Récupérer la commande et le livreur sélectionné
            var commande = await _commandeService.FindById(idCommande);
            
            if (commande == null)
            {
                return NotFound();
            }
            _commandeService.CommandeDeclarerRecu(commande);
            await _commandeService.Update(commande);
            _commandeService.ValiderCommande(commande);
            TempData["Message"] = "Commande est declarer recu mis à jour avec succès!";
            return RedirectToAction(nameof(Index));  // Rediriger vers la liste des commandes
        }
        
        [HttpPost]
        public async Task<IActionResult> ValiderCommande(int idCommande, int livreurId)
        {
            // Récupérer la commande et le livreur sélectionné
            var commande = await _commandeService.FindById(idCommande);
            var livreur = await _livreurService.FindById(livreurId);
            
            if (commande == null || livreur == null)
            {
                return NotFound();
            }
            livreur.EtatLivreur = EtatLivreur.EnCourse;
            await _livreurService.Update(livreur);
            _commandeService.ValiderCommande(commande);
            TempData["Message"] = "Commande validée et livreur mis à jour avec succès!";
            return RedirectToAction(nameof(Index));  // Rediriger vers la liste des commandes
        }

        // Action pour mettre la commande en attente
        public async Task<IActionResult> EnAttenteCommande(int id)
        {
            // Attendez la complétion de la tâche pour obtenir le résultat
            var commande = await _commandeService.FindById(id);
            
            if (commande == null)
            {
                return NotFound();
            }
            
            await _commandeService.MettreEnAttente(commande);  // Si cette méthode est asynchrone aussi, utilisez await
            TempData["Message"] = "Commande mise en attente.";
            return RedirectToAction(nameof(Index));  // Redirige vers la liste des commandes
        }


        public async Task<IActionResult> TraiterCommande()
        {
            var livreurs = await _livreurService.GetLivreursDispo();  // Utilisation de await ici

            if (livreurs == null || !livreurs.Any())
            {
                ViewBag.MessageErreur = "Aucun livreur disponible pour le moment.";
                livreurs = new List<Livreur>();
            }
            return View(livreurs);  // Passer la liste des livreurs en tant que Model
        }

    }
}
