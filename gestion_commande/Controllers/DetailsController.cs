// using gestion_commande.Model;
// using gestion_commande.Services.Interfaces;
// using Microsoft.AspNetCore.Mvc;
// using System.Threading.Tasks;

// namespace gestion_commande.Controllers
// {
//     public class DetailController : Controller
//     {
//         private readonly IDetailsModel _detailModel;
        
//         public IActionResult Index()
//         {
//             var details = _detailModel.GetDetails();
//             return View(details);
//         }
//         // Injecter le modèle detail (le service detailModel)
//         public DetailController(IDetailsModel detailModel)
//         {
//             _detailModel = detailModel;
//         }

//         // Action pour lister tous les details
//         public async Task<IActionResult> kiki()
//         {
//             var details = await _detailModel.FindAll();
//             return View(details); // Retourner la vue avec la liste des details
//         }

//         // Action pour afficher les détails d'un detail par son ID
//         public async Task<IActionResult> Details(int id)
//         {
//             var detail = await _detailModel.FindById(id);
//             if (detail == null)
//             {
//                 return NotFound();
//             }

//             return View(detail); // Retourner la vue de détails
//         }

//         // Action pour afficher le formulaire de création d'un detail
//         public IActionResult Create()
//         {
//             return View();
//         }

//         // Action pour enregistrer un detail (POST)
//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> Create(Detail detail)
//         {
//             if (ModelState.IsValid)
//             {
//                 await _detailModel.Create(detail);
//                 return RedirectToAction(nameof(Index)); // Rediriger vers la liste des details après enregistrement
//             }
//             return View(detail); // Retourner la vue avec le formulaire si la validation échoue
//         }

//         // Action pour afficher le formulaire d'édition d'un detail
//         public async Task<IActionResult> Edit(int id)
//         {
//             var detail = await _detailModel.FindById(id);
//             if (detail == null)
//             {
//                 return NotFound();
//             }

//             return View(detail); // Retourner la vue d'édition
//         }

//         // Action pour mettre à jour un detail (POST)
//         [HttpPost]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> Edit(int id, Detail detail)
//         {
//             if (id != detail.Id)
//             {
//                 return NotFound();
//             }

//             if (ModelState.IsValid)
//             {
//                 await _detailModel.Update(detail);
//                 return RedirectToAction(nameof(Index)); // Rediriger vers la liste des details après mise à jour
//             }
//             return View(detail); // Retourner la vue avec les informations de l'édition
//         }

//         // Action pour supprimer un detail
//         public async Task<IActionResult> Delete(int id)
//         {
//             var detail = await _detailModel.FindById(id);
//             if (detail == null)
//             {
//                 return NotFound();
//             }

//             return View(detail); // Retourner la vue de confirmation de suppression
//         }

//         // Action pour supprimer un detail (POST)
//         [HttpPost, ActionName("Delete")]
//         [ValidateAntiForgeryToken]
//         public async Task<IActionResult> DeleteConfirmed(int id)
//         {
//             await _detailModel.Delete(id);
//             return RedirectToAction(nameof(Index)); // Rediriger vers la liste des details après suppression
//         }
//     }
// }
