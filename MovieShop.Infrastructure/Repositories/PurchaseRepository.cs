using MovieShop.Core.Entities;
using MovieShop.Core.RepositoryInterfaces;
using MovieShop.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

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

    }
}
