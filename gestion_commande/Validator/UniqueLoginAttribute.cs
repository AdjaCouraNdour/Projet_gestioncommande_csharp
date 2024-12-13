using System.ComponentModel.DataAnnotations;
using gestion_commande.Services.Interfaces;

namespace gestion_commande.Validator
{
    public class UniqueLoginAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value ,ValidationContext validationContext)
        {
            var userService = (IUserService)validationContext.GetService(typeof(IUserService));
            var login = (string)value;
            
            if (userService.GetUsers().Any(c => c.Login == login))
            {
                return new ValidationResult("Cet Login est deja existant.");
            }
            
        return ValidationResult.Success;
        } 
    }
}