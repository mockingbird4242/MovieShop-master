using MovieShop.Core.Entities;
using MovieShop.Core.RepositoryInterfaces;
using MovieShop.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace MovieShop.Infrastructure.Repositories
{
    public class PurchaseRepository : EfRepository<Purchase>, IPurchaseRepository
    {
        public PurchaseRepository(MovieShopDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<decimal?> GetPriceByMovieId(int id)
        {
            var movie = await _dbContext.Movies.FirstOrDefaultAsync(u => u.Id == id);
            decimal? price = movie.Price;
            return price;
        }
        public async Task<IEnumerable<Purchase>> GetAllPurchases(int pageSize = 30, int pageIndex = 1)
        {
            var purchases = await _dbContext.Purchases.Include(m => m.Movie).OrderByDescending(p => p.PurchaseDateTime)
                .Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync();
            return purchases;
        }

    }
}
