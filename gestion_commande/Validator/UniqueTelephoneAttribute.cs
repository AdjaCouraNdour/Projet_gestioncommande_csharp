using System.ComponentModel.DataAnnotations;
using gestion_commande.Services.Interfaces;

namespace gestion_commande.Validator
{
    public class UniqueTelephoneAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value ,ValidationContext validationContext)
        {
            var userService = (IUserService)validationContext.GetService(typeof(IUserService));
            var telephone = (string)value;
            
            if (userService.GetUsers().Any(c => c.Telephone == telephone))
            {
                return new ValidationResult("Ce Telephone est deja existant.");
            }
         
        return ValidationResult.Success;
        } 
    }
}