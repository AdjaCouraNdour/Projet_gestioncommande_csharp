using System.ComponentModel.DataAnnotations;
using gestion_commande.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace gestion_commande.Validator
{
    public class UniqueNomCompletAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value ,ValidationContext validationContext)
        {
            var LivreurService = (ILivreurService)validationContext.GetService(typeof(ILivreurService));
            var nomComplet = (string)value;
            
            if (LivreurService.GetLivreurs().Any(l => l.NomComplet == nomComplet))
            {
                return new ValidationResult("Ce NomComplet est deja existant.");
            }
         
        return ValidationResult.Success;
        } 
    }
}