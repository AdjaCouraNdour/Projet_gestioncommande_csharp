using System.ComponentModel.DataAnnotations;
using gestion_commande.Services.Interfaces;

namespace gestion_commande.Validator
{
    public class UniqueUsernameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value ,ValidationContext validationContext)
        {
            var userService = (IUserService)validationContext.GetService(typeof(IUserService));
            var username = (string)value;
            
            if (userService.GetUsers().Any(c => c.Username == username))
            {
                return new ValidationResult("Ce Username est deja existant.");
            }
         
        return ValidationResult.Success;
        } 
    }
}