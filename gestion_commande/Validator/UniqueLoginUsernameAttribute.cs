using System.ComponentModel.DataAnnotations;
using gestion_commande.Services.Interfaces;

namespace gestion_commande.Validator
{
    public class UniqueLoginUsernameAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value ,ValidationContext validationContext)
        {
            var clientService = (IClientService)validationContext.GetService(typeof(IClientService));
            var login = (string)value;
            
            if (clientService.GetClients().Any(c => c.Login == login))
            {
                return new ValidationResult("Cet Login est deja existant.");
            }
            
        return ValidationResult.Success;
        } 
    }
}