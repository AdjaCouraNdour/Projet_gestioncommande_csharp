using gestion_commande.Models;
using gestion_commande.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using gestion_commande.Enums;

namespace gestion_commande.Controllers
{
    public class AuthentificationController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AuthentificationController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                ViewBag.Message = "Veuillez remplir tous les champs.";
                return View();
            }
            var user = _context.Users.FirstOrDefault(u => u.Login == login && u.Password == password);
            if (user != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Login),
                    new Claim("UserId", user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.UserRole.ToString()) // Ajouter le rôle comme revendication

                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var principal = new ClaimsPrincipal(identity);

                // Se connecter et ajouter les informations dans le cookie
                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                // Rediriger vers une page sécurisée après la connexion
                if (user.UserRole == UserRole.Client)
                {
                    return RedirectToAction("ListeDeMesCommandes", "Commande"); 
                }else{
                    return RedirectToAction("Index", "Produit"); 
                }
            }
            else
            {
                ViewBag.Message = "Nom d'utilisateur ou mot de passe incorrect.";
                return View();
            }
        }

        // Page d'accès refusé (optionnel si vous souhaitez avoir cette page)
        public IActionResult AccessDenied()
        {
            return View();
        }

        // Déconnexion de l'utilisateur
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login"); // Rediriger vers la page de login après la déconnexion
        }
    }
}
