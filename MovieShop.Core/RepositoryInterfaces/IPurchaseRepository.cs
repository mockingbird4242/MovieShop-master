using MovieShop.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MovieShop.Core.RepositoryInterfaces
{
    public interface IPurchaseRepository : IAsyncRepository<Purchase>
    {
        Task<decimal?> GetPriceByMovieId(int id);
        Task<IEnumerable<Purchase>> GetAllPurchases(int pageSize = 30, int pageIndex = 0);

    }
}