using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MovieShop.Core.Entities;
using MovieShop.Core.RepositoryInterfaces;
using MovieShop.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using MovieShop.Core.Models.Response;
using ApplicationCore.Helpers;
using ApplicationCore.Exceptions;

namespace MovieShop.Infrastructure.Repositories
{
    public class MovieRepository : EfRepository<Movie>, IMovieRepository
    {
        public MovieRepository(MovieShopDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IEnumerable<MovieRatingResponseModel>> GetTopRatedMovies()
        {
            var topRatedMovies = await _dbContext.Reviews.Include(m => m.Movie)
                                                 .GroupBy(r => new
                                                 {
                                                     Id = r.MovieId,
                                                     r.Movie.PosterUrl,
                                                     r.Movie.Title,
                                                     r.Movie.ReleaseDate
                                                 })
                                                 .OrderByDescending(g => g.Average(m => m.Rating))
                                                 .Select(m => new MovieRatingResponseModel
                                                 {
                                                     Id = m.Key.Id,
                                                     PosterUrl = m.Key.PosterUrl,
                                                     Title = m.Key.Title,
                                                     ReleaseDate = m.Key.ReleaseDate,
                                                     Rating = m.Average(x => x.Rating)
                                                 })
                                                 .Take(10)
                                                 .ToListAsync();

            return topRatedMovies;


        }

        public async Task<IEnumerable<Movie>> GetTopRevenueMovies()
        {
            return await _dbContext.Movies.OrderByDescending(m => m.Revenue).Take(25).ToListAsync();
        }

        public override async Task<Movie> GetByIdAsync(int id)
        {
            return await _dbContext.Movies.Include(m => m.MovieCasts).ThenInclude(m => m.Cast).Include(m => m.Genres).FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<IEnumerable<Review>> GetMovieReviews(int id)
        {
            var reviews = await _dbContext.Reviews.Where(r => r.MovieId == id).Include(r => r.User)
                                          .Select(r => new Review
                                          {
                                              UserId = r.UserId,
                                              Rating = r.Rating,
                                              MovieId = r.MovieId,
                                              ReviewText = r.ReviewText,
                                          }).ToListAsync();
            return reviews;
        }

        public async Task<IEnumerable<Purchase>> GetMoviePurchases(int id)
        {
            var purchases = await _dbContext.Purchases.Where(r => r.UserId == id).Include(r => r.Movie)
                .Select(r => new Purchase
                {
                    UserId = r.UserId,
                    MovieId = r.MovieId,
                    PurchaseNumber = r.PurchaseNumber,
                    TotalPrice = r.TotalPrice,
                    PurchaseDateTime = r.PurchaseDateTime
                }).ToListAsync();
            return purchases;
        }

        public async Task<Movie> GetMovieByTitle(string title)
        {
            var movie = await _dbContext.Movies.FirstOrDefaultAsync(m => m.Title == title);
            return movie;
        }

        public async Task<IEnumerable<Movie>> GetMovies(MovieParameters movieParameters)
        {
            var movies = await _dbContext.Movies.Include(m => m.MovieCasts).ThenInclude(m => m.Cast).Include(m => m.Genres).OrderBy(on => on.Id).Skip((movieParameters.PageNumber - 1) * movieParameters.PageSize).Take(movieParameters.PageSize).ToListAsync();
            return movies;
        }

        public async Task<IEnumerable<Movie>> GetMovies()
        {
            return await _dbContext.Movies.OrderBy(m => m.Id).Take(30).ToListAsync();


        }

        public async Task<PaginatedList<Movie>> GetMoviesByGenre(int genreId, int pageSize = 25, int page = 1)
        {
            var totalMoviesCountByGenre =
                await _dbContext.Genres.Include(g => g.Movies).Where(g => g.Id == genreId).SelectMany(g => g.Movies)
                    .CountAsync();

            if (totalMoviesCountByGenre == 0)
            {
                throw new NotFoundException("NO Movies found for this genre");
            }
            var movies = await _dbContext.Genres.Include(g => g.Movies).Where(g => g.Id == genreId)
                .SelectMany(g => g.Movies)
                .OrderByDescending(m => m.Revenue).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

            return new PaginatedList<Movie>(movies, totalMoviesCountByGenre, page, pageSize);
        }

        public async Task<Movie> GetMovieById(int id)
        {
            var movie = await _dbContext.Movies.FirstOrDefaultAsync(m => m.Id == id);
            return movie;
        }

        public async Task<Cast> GetCastById(int id)
        {
            var countCastById =
                await _dbContext.MovieCasts.Where(mc => mc.MovieId == id).Select(c => c.Cast).CountAsync();

            if (countCastById== 0)
            {
                throw new NotFoundException("NO Cast found for this movie");
            }
            //var cast = await _dbContext.MovieCasts.Include(mc => mc.Cast).Where(mc => mc.MovieId == id).ToListAsync();
            var cast = await _dbContext.MovieCasts.Include(g => g.Cast).Where(g => g.MovieId == id)
                .Select(g => g.Cast).ToListAsync();
            var cast2 = await _dbContext.Casts.Where(c => c.Id == id).Include(c => c.MovieCasts)
                                      .ThenInclude(c => c.Movie).FirstOrDefaultAsync();

            return cast2;
        }
    }
}
