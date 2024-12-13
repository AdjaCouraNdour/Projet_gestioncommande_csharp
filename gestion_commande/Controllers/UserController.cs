using gestion_commande.Models;
using gestion_commande.Enums;
using gestion_commande.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;

namespace gestion_commande.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
    
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

          public IActionResult Login()
        {
            return View(); // Affiche une vue d'accueil ou tableau de bord
        }
        public async Task<IActionResult> Index(int page = 1, int pageSize = 3)
        {
            // Fetch users from the service
            var users = await _userService.GetUsersByPaginate(page, pageSize);
            // Pass the users to the view
            return View(users);
        }

         [HttpGet]
        public IActionResult FormUser()
        {
            ViewBag.UserRoles = GetRolesAsSelectList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FormUser([Bind("Id,Nom,Prenom,Telephone.Address,Login,Password,UserRole")] User user)
        {
            if (ModelState.IsValid)
            {
                var userAdded = await _userService.Create(user);
                TempData["Message"] = "user créé avec succès!";
                return RedirectToAction(nameof(Index));

            }
            return View(user);
        }

        public SelectList GetRolesAsSelectList()
        {
            // Convertit l'énumération en une liste de paires valeur-texte
            var roles = Enum.GetValues(typeof(UserRole))
                            .Cast<UserRole>()
                            .Select(role => new SelectListItem
                            {
                                Value = role.ToString(),
                                Text = role.ToString()
                            }).ToList();

            return new SelectList(roles, "Value", "Text");
        }
        // Action pour afficher les détails d'un user par son ID
        public async Task<IActionResult> DetailsUser(int id)
        {
            var user = await _userService.FindById(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user); // Retourner la vue de détails
        }

        // Action pour afficher le formulaire de création d'un user
        public IActionResult Create()
        {
            return View();
        }

        // Action pour enregistrer un user (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                await _userService.Create(user);
                return RedirectToAction(nameof(Index)); // Rediriger vers la liste des users après enregistrement
            }
            return View(user); // Retourner la vue avec le formulaire si la validation échoue
        }

        // Action pour afficher le formulaire d'édition d'un user
        public async Task<IActionResult> Edit(int id)
        {
            var user = await _userService.FindById(id);
            if (user == null)
            {
                return NotFound();
            }

            return View(user); // Retourner la vue d'édition
        }

        // Action pour mettre à jour un user (POST)
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,  User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                await _userService.Update(user);
                return RedirectToAction(nameof(Index)); // Rediriger vers la liste des users après mise à jour
            }
            return View(user); // Retourner la vue avec les informations de l'édition
        }

        // Action pour supprimer un user
        public async Task<IActionResult> Delete(int id)
        {
            var user = await _userService.FindById(id);
            if (user == null)
            {
                return NotFound();
            }

            await _userService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
