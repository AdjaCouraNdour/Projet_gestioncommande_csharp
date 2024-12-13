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
    public class UserController : Controller
    {
        private readonly ApplicationDbContext _context;

        private readonly IUserService _userService;
    
        public UserController(IUserService userService,ApplicationDbContext context)
        {
            _userService = userService;
            _context = context;
        }
        
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
      public IActionResult Login(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                ViewBag.Message = "Veuillez remplir tous les champs.";
                return View();
            }
            var user = _context.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
            if (user != null)
            {
                ViewBag.Message = "Connexion réussie !";
                return RedirectToAction("Index", "Produit"); // Redirection vers une autre page
            }
            else
            {
                ViewBag.Message = "Nom d'utilisateur ou mot de passe incorrect.";
                return View();
            }
        }
        

        
        public async Task<IActionResult> Index(int page = 1, int pageSize = 3)
        {
            // Fetch users from the service
            var users = await _userService.GetUsersByPaginate(page, pageSize);
            // Pass the users to the view
            return View(users);
        }

     
        public IActionResult FormUser()
        {
            ViewBag.UserRoles = GetRolesAsSelectList();
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> FormUser([Bind("Email,Login,Telephone,Address,Password")] User user)
        {
            if (ModelState.IsValid)
            {
                var userAdded = await _userService.Create(user);
                TempData["Message"] = "user créé avec succès!";
                return RedirectToAction(nameof(Index));

            }
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> Create(User user)
        {
            if (ModelState.IsValid)
            {
                user.UserRole = UserRole.Client;
                // Créer le client associé à l'utilisateur
                var client = new Client
                {
                    User = user // Associer le client à l'utilisateur
                };

                // Ajouter l'utilisateur et le client dans le contexte
                _context.Users.Add(user);
                _context.Clients.Add(client);
                 user.ClientId = client.Id; 
                // Sauvegarder dans la base de données
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", "Home"); // Redirection après création
            }

            return View(user); // Si la validation échoue, retourner à la même vue
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
