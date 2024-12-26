using gestion_commande.Models;
using Microsoft.EntityFrameworkCore;

namespace gestion_commande.Core{

    public class PaginationCommandeModel : PaginationModel<Commande>
    {
        public Client Client { get; set; }
        protected PaginationCommandeModel(List<Commande> items, int totalItems, int pageSize, int currentPage, Client client)
        : base(items, totalItems, pageSize, currentPage)
        {
            Client = client;
        }

        public static async Task<PaginationCommandeModel> PaginateCommande(IQueryable<Commande> data, int pageSize, int currentPage, Client client)
        {   
            var pageModel = await PaginationModel<Commande>.Paginate(data, pageSize, currentPage);
            return new PaginationCommandeModel( pageModel.Items, pageModel.TotalItems, pageModel.PageSize, pageModel.CurrentPage, client);
        }
    }
}