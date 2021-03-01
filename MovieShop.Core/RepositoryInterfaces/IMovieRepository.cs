using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Helpers;
using MovieShop.Core.Entities;
using MovieShop.Core.Models.Response;

namespace MovieShop.Core.RepositoryInterfaces
{
    public interface IMovieRepository : IAsyncRepository<Movie>
    {
        Task<IEnumerable<MovieRatingResponseModel>> GetTopRatedMovies();
        Task<IEnumerable<Movie>> GetTopRevenueMovies();
        Task<IEnumerable<Review>> GetMovieReviews(int id);
        Task<IEnumerable<Purchase>> GetMoviePurchases(int id);
        Task<Movie> GetMovieByTitle(string title);
        Task<IEnumerable<Movie>> GetMovies();
        Task<IEnumerable<Movie>> GetMovies(MovieParameters movieParameters);
        Task<PaginatedList<Movie>> GetMoviesByGenre(int genreId, int pageSize = 25, int page = 1);
        Task<Movie> GetMovieById(int id);
        Task<Cast> GetCastById(int id);

    }
}
