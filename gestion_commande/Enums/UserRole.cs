

using System.Security.Claims;

namespace gestion_commande.Enums
{
    public enum UserRole
    {
        RS,Secretaire,Comptable,Client

    }
    
}
 public static class ClaimsPrincipalExtensions
    {
        public static string GetUserRole(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Role)?.Value; // Ou un autre type personnalisé si nécessaire
        }
    }