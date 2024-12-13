
namespace gestion_commande.Models
{
    public class AbstractEntity
    {
        public DateTime CreateAt { get; set; } = DateTime.Now;
        public DateTime UpdateAt { get; set; } = DateTime.Now;
    }
}