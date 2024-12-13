using gestion_commande.Models;
using gestion_commande.Services;
using gestion_commande.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace gestion_commande.Controllers
{
    public class PaiementController : Controller
    {
        private readonly IPaiementService _paiementService;
        private readonly ICommandeService _commandeService;

        
        // Injecter le modèle paiement (le service paiementService)
        public PaiementController(IPaiementService paiementService,ICommandeService commandeService)
        {
            _paiementService = paiementService;
            _commandeService = commandeService;

        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 3)
        {
            // Fetch paiements from the service
            var paiements = await _paiementService.GetPaiementsByPaginate(page, pageSize);
            // Pass the paiements to the view
            return View(paiements);
        }
        // Action pour afficher les détails d'un paiement par son ID
        public async Task<IActionResult> Details(int id)
        {
            var paiement = await _paiementService.FindById(id);
            if (paiement == null)
            {
                return NotFound();
            }

            return View(paiement); // Retourner la vue de détails
        }

        // Action pour afficher le formulaire de création d'un paiement
        public IActionResult Create()
        {
            return View();
        }

        // Action pour enregistrer un paiement (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Paiement paiement)
        {
            if (ModelState.IsValid)
            {
                await _paiementService.Create(paiement);
                return RedirectToAction(nameof(Index)); // Rediriger vers la liste des paiements après enregistrement
            }
            return View(paiement); // Retourner la vue avec le formulaire si la validation échoue
        }

        // Action pour afficher le formulaire d'édition d'un paiement
        public async Task<IActionResult> Edit(int id)
        {
            var paiement = await _paiementService.FindById(id);
            if (paiement == null)
            {
                return NotFound();
            }

            return View(paiement); // Retourner la vue d'édition
        }

        // Action pour mettre à jour un paiement (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Paiement paiement)
        {
            if (id != paiement.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _paiementService.Update(paiement);
                return RedirectToAction(nameof(Index)); // Rediriger vers la liste des paiements après mise à jour
            }
            return View(paiement); // Retourner la vue avec les informations de l'édition
        }

        // Action pour supprimer un paiement
        public async Task<IActionResult> Delete(int id)
        {
            var paiement = await _paiementService.FindById(id);
            if (paiement == null)
            {
                return NotFound();
            }

            await _paiementService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

    
        public async Task<IActionResult> Paiementscommande(int Id)
        {
             var paiementsCommande = await _paiementService.GetPaiementsCommande(Id);         

            return View(paiementsCommande);
        }

         // Action pour afficher les paiements d'une commande
        public async Task<IActionResult> FormPaiement(int commandeId)
        {
            var commande = await _commandeService.FindById(commandeId);
            if (commande == null)
            {
                return NotFound();
            }

            ViewBag.commande = commande;
            return View(new Paiement { CommandeId = commande.Id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FormPaiement([Bind("Montant, commandeId")] Paiement paiement)
        {
            if (!ModelState.IsValid)
            {
                var commande = await _commandeService.FindById(paiement.CommandeId);
                ViewBag.commande = commande;
                return View(paiement);
            }

            var commandeToUpdate = await _commandeService.FindById(paiement.CommandeId);
            if (commandeToUpdate != null)
            {
                await _paiementService.Create(paiement);
                await _commandeService.Update(commandeToUpdate);
                TempData["Message"] = "Paiement enregistré avec succès!";
                return RedirectToAction("commandesClient", "commande");
            }

            return View(paiement);
        }

    }
}
