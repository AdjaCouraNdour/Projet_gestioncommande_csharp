using gestion_commande.Models;
using gestion_commande.Services;
using gestion_commande.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace gestion_commande.Controllers
{
    public class ProduitController : Controller
    {
        private readonly IProduitService _produitService;
        
        public ProduitController(IProduitService produitService)
        {
            _produitService = produitService;
        }

        public async Task<IActionResult> Index(int page = 1, int pageSize = 3)
        {
            // Fetch produits from the service
            var produits = await _produitService.GetProduitsByPaginate(page, pageSize);
            // Pass the produits to the view
            return View(produits);
        }

        [HttpGet]
        public IActionResult FormProduit()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FormProduit([Bind("Libelle,Prix,QteStock")] Produit produit)
        {
            if (ModelState.IsValid)
            {
                var produitAdded = await _produitService.Create(produit);
                TempData["Message"] = "produit créé avec succès!";
                return RedirectToAction(nameof(Index));

            }
            return View(produit);
        }

        // Action pour afficher les détails d'un produit par son ID
        public async Task<IActionResult> DetailsProduit(int id)
        {
            var produit = await _produitService.FindById(id);
            if (produit == null)
            {
                return NotFound();
            }

            return View(produit); // Retourner la vue de détails
        }

        // Action pour afficher le formulaire d'édition d'un produit
        public async Task<IActionResult> Edit(int id)
        {
            var produit = await _produitService.FindById(id);
            if (produit == null)
            {
                return NotFound();
            }

            return View(produit); // Retourner la vue d'édition
        }

        // Action pour mettre à jour un produit (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Produit produit)
        {
            if (id != produit.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _produitService.Update(produit);
                return RedirectToAction(nameof(Index)); // Rediriger vers la liste des produits après mise à jour
            }
            return View(produit); // Retourner la vue avec les informations de l'édition
        }

        // Action pour supprimer un produit
        public async Task<IActionResult> Delete(int id)
        {
            var produit = await _produitService.FindById(id);
            if (produit == null)
            {
                return NotFound();
            }

            await _produitService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
