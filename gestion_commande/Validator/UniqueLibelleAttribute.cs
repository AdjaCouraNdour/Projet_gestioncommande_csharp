using System.ComponentModel.DataAnnotations;
using gestion_commande.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace gestion_commande.Validator
{
    public class UniqueLibelleAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value ,ValidationContext validationContext)
        {
            var ProduitService = (IProduitService)validationContext.GetService(typeof(IProduitService));
            var libelle = (string)value;
            
            if (ProduitService.GetProduits().Any(a => a.Libelle == libelle))
            {
                return new ValidationResult("Ce libelle est deja existant.");
            }
         
        return ValidationResult.Success;
        } 
    }
}