using System.ComponentModel.DataAnnotations;
using gestion_commande.Services.Interfaces;

namespace gestion_commande.Validator
{
    public class UniqueEmailAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value ,ValidationContext validationContext)
        {
            var userService = (IUserService)validationContext.GetService(typeof(IUserService));
            var email = (string)value;
            
            if (userService.GetUsers().Any(u => u.Email == email))
            {
                return new ValidationResult("Cet Email est deja existant.");
            }
            
        return ValidationResult.Success;
        } 
    }
}