using gestion_commande.Models;
using gestion_commande.Enums;
using gestion_commande.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using gestion_commande.Data;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace gestion_commande.Controllers
{
    public class LivreurController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILivreurService _livreurService;
    
        public LivreurController(ILivreurService livreurService,ApplicationDbContext context)
        {
            _livreurService = livreurService;
            _context = context;
        }        
        public async Task<IActionResult> Index(int page = 1, int pageSize = 3)
        {
            // Fetch livreurs from the service
            var livreurs = await _livreurService.GetLivreursByPaginate(page, pageSize);
            // Pass the livreurs to the view
            return View(livreurs);
        }

     
        public IActionResult FormLivreur()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FormLivreur([Bind("NomComplet,Telephone,Email")] Livreur livreur)
        {
            if (ModelState.IsValid)
            {
                livreur.EtatLivreur = EtatLivreur.Disponible;
                var livreurAdded = await _livreurService.Create(livreur);
                TempData["Message"] = "livreur créé avec succès!";
                return RedirectToAction(nameof(Index));

            }
            return View(livreur);
        }       
        public SelectList GetEtatAsSelectList()
        {
            // Convertit l'énumération en une liste de paires valeur-texte
            var Etat = Enum.GetValues(typeof(EtatLivreur))
                            .Cast<EtatLivreur>()
                            .Select(role => new SelectListItem
                            {
                                Value = role.ToString(),
                                Text = role.ToString()
                            }).ToList();

            return new SelectList(Etat, "Value", "Text");
        }
        // Action pour afficher les détails d'un livreur par son ID
        public async Task<IActionResult> DetailsLivreur(int id)
        {
            var livreur = await _livreurService.FindById(id);
            if (livreur == null)
            {
                return NotFound();
            }

            return View(livreur); // Retourner la vue de détails
        }

        // Action pour afficher le formulaire d'édition d'un livreur
        public async Task<IActionResult> Edit(int id)
        {
            var livreur = await _livreurService.FindById(id);
            if (livreur == null)
            {
                return NotFound();
            }

            return View(livreur); // Retourner la vue d'édition
        }

        // Action pour mettre à jour un livreur (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  Livreur livreur)
        {
            if (id != livreur.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _livreurService.Update(livreur);
                return RedirectToAction(nameof(Index)); // Rediriger vers la liste des livreurs après mise à jour
            }
            return View(livreur); // Retourner la vue avec les informations de l'édition
        }

        // Action pour supprimer un livreur
        public async Task<IActionResult> Delete(int id)
        {
            var livreur = await _livreurService.FindById(id);
            if (livreur == null)
            {
                return NotFound();
            }

            await _livreurService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
