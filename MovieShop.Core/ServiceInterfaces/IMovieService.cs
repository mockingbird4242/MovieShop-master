using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using ApplicationCore.Helpers;
using MovieShop.Core.Entities;
using MovieShop.Core.Models.Request;
using MovieShop.Core.Models.Response;

namespace MovieShop.Core.ServiceInterfaces
{
    public interface IMovieService
    {
        Task<MovieDetailsResponseModel> GetMovieById(int id);
        Task<IEnumerable<MovieCardResponseModel>> GetTop25GrossingMovies();
        Task<IEnumerable<ReviewResponseModel>> GetReviewsForMovie(int id);
        Task<bool> CreateMovie(MovieDetailsRequestModel movieDetailsRequestModel);
        Task<List<MovieDetailsResponseModel>> GetAllMovies(MovieParameters movieParameters);
        Task<List<MovieDetailsResponseModel>> GetAllMovies();

        Task<List<MovieRatingResponseModel>> GetTop10RatedMovies();
        Task<PaginatedList<BasicMovieResponseModel>> GetMoviesByGenre(int genreId, int pageSize = 25, int page = 1);
        Task<bool> UpdateMovie(int id, MovieDetailsRequestModel movieDetailsRequestModel);
        Task<PagedResultSet<BasicMovieResponseModel>> GetAllMoviePurchasesByPagination(int pageSize = 20, int page = 1);
        Task<BasicCastModel> GetCastById(int id);
    }
}
