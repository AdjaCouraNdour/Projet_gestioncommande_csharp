using Microsoft.AspNetCore.Mvc;
using gestion_commande.Models;
using gestion_commande.Data;
using System.Linq;

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
        public IActionResult Login(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                ViewBag.Message = "Veuillez remplir tous les champs.";
                return View();
            }

            // Vérification des identifiants dans la base de données
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
    }
}
