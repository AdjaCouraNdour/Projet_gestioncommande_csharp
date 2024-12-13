using Microsoft.AspNetCore.Mvc;
using gestion_commande.Models;

namespace gestion_commande.Controllers
{
    public class AuthentificationController : Controller
    {
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(User model)
        {
            string correctUsername = "kiki";
            string correctPassword = "k";

            if (model.Username == correctUsername && model.Password == correctPassword)
            {
                ViewBag.Message = "Connexion réussie !";
                return RedirectToAction("Index", "User"); 
            }
            else
            {
                ViewBag.Message = "Nom d'utilisateur ou mot de passe incorrect.";
                return View();
            }
        }
    }
}
