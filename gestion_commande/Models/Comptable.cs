
using System.ComponentModel.DataAnnotations;
using gestion_commande.Enums;
using gestion_commande.Validator;

namespace gestion_commande.Models
{
    public class Comptable:AbstractEntity
    {        
     public int Id { get; set; }

       public Comptable()
        {
            CreateAt = DateTime.Now;
            UpdateAt = DateTime.Now;
        }
    }
}