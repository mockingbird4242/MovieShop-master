using System;
using System.Collections.Generic;
using System.Text;
using MovieShop.Core.Entities;
using MovieShop.Core.ServiceInterfaces;
using MovieShop.Infrastructure.Repositories;
using MovieShop.Core.RepositoryInterfaces;
using MovieShop.Core.Models.Response;
using System.Threading.Tasks;
using MovieShop.Core.Models.Request;
using MovieShop.Core.Exceptions;
using ApplicationCore.Helpers;
using AutoMapper;

namespace MovieShop.Infrastructure.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository; //Dependency Injection by constructor injection; why readonly: only change in declaration(inside constructor)
        //MovieService movieservice = new MovieService (); will not compile because we have a parameter(a class that implement IMovieRepository)
        //MovieService movieservice = new MovieService (new MovieRepository()); will compile becuase MovieRepository implements IMovieRepository
        private readonly IMapper _mapper;
        private readonly IPurchaseRepository _purchaseRepository;
        public MovieService(IMovieRepository movieRepository, IMapper mapper, IPurchaseRepository purchaseRepository) // Constructor with an Interface as parameter: pass a class that implement this Interface
        {
            _mapper = mapper;
            _movieRepository = movieRepository;
            _purchaseRepository = purchaseRepository;// 
        }

        public async Task<bool> CreateMovie(MovieDetailsRequestModel movieDetailsRequestModel)
        {
            var dbMovie = await _movieRepository.GetMovieByTitle(movieDetailsRequestModel.Title);
            if (dbMovie != null)
            {
                throw new MovieConflictException("Movie already exists");
            }
            var movie = new Movie
            {
                Title = movieDetailsRequestModel.Title,
                Overview = movieDetailsRequestModel.Overview,
                Tagline = movieDetailsRequestModel.Tagline,
                Budget = movieDetailsRequestModel.Budget,
                Revenue = movieDetailsRequestModel.Revenue,
                ImdbUrl = movieDetailsRequestModel.ImdbUrl,
                TmdbUrl = movieDetailsRequestModel.TmdbUrl,
                PosterUrl = movieDetailsRequestModel.PosterUrl,
                BackdropUrl = movieDetailsRequestModel.BackdropUrl,
                OriginalLanguage = movieDetailsRequestModel.OriginalLanguage,
                ReleaseDate = movieDetailsRequestModel.ReleaseDate,
                RunTime = movieDetailsRequestModel.RunTime,
                Price = movieDetailsRequestModel.Price

            };
            var createdMovie = await _movieRepository.AddAsync(movie);
            if (createdMovie != null && createdMovie.Id > 0)
            {
                return true;
            }
            return false;

        }
        public async Task<bool> UpdateMovie(int id, MovieDetailsRequestModel movieDetailsRequestModel)
        {
            var dbMovie = await _movieRepository.GetMovieById(id);
            if (dbMovie == null)
            {
                throw new MovieConflictException("Movie does not exist");
            }

            dbMovie.Title = movieDetailsRequestModel.Title;
            dbMovie.Overview = movieDetailsRequestModel.Overview;
            dbMovie.Tagline = movieDetailsRequestModel.Tagline;
            dbMovie.Budget = movieDetailsRequestModel.Budget;
            dbMovie.Revenue = movieDetailsRequestModel.Revenue;
            dbMovie.ImdbUrl = movieDetailsRequestModel.ImdbUrl;
            dbMovie.TmdbUrl = movieDetailsRequestModel.TmdbUrl;
            dbMovie.PosterUrl = movieDetailsRequestModel.PosterUrl;
            dbMovie.BackdropUrl = movieDetailsRequestModel.BackdropUrl;
            dbMovie.OriginalLanguage = movieDetailsRequestModel.OriginalLanguage;
            dbMovie.ReleaseDate = movieDetailsRequestModel.ReleaseDate;
            dbMovie.RunTime = movieDetailsRequestModel.RunTime;
            dbMovie.Price = movieDetailsRequestModel.Price;
            await _movieRepository.UpdateAsync(dbMovie);
            return true;

        }

        public async Task<MovieDetailsResponseModel> GetMovieById(int id)
        {
            var movieDetails = new MovieDetailsResponseModel();
            var movie = await _movieRepository.GetByIdAsync(id);

            // map movie entity to MovieDetailsResponseModel
            movieDetails.Id = movie.Id;
            movieDetails.PosterUrl = movie.PosterUrl;
            movieDetails.Title = movie.Title;
            movieDetails.Overview = movie.Overview;
            movieDetails.Tagline = movie.Tagline;
            movieDetails.Budget = movie.Budget;
            movieDetails.Revenue = movie.Revenue;
            movieDetails.ImdbUrl = movie.ImdbUrl;
            movieDetails.TmdbUrl = movie.TmdbUrl;
            movieDetails.BackdropUrl = movie.BackdropUrl;
            movieDetails.OriginalLanguage = movie.OriginalLanguage;
            movieDetails.ReleaseDate = movie.ReleaseDate;
            movieDetails.RunTime = movie.RunTime;
            movieDetails.Price = movie.Price;

            movieDetails.Genres = new List<GenreModel>();
            movieDetails.Casts = new List<CastResponseModel>();

            foreach (var genre in movie.Genres)
            {
                movieDetails.Genres.Add(new GenreModel { Id = genre.Id, Name = genre.Name });
            }

            foreach (var cast in movie.MovieCasts)
            {
                movieDetails.Casts.Add(new CastResponseModel
                {
                    Id = cast.CastId,
                    Character = cast.Character,
                    Name = cast.Cast.Name,
                    ProfilePath = cast.Cast.ProfilePath
                });
            }

            return movieDetails;
        }


        public async Task<IEnumerable<ReviewResponseModel>> GetReviewsForMovie(int id)
        {
                var reviews = await _movieRepository.GetMovieReviews(id);
                var reviewDetails = new List<ReviewResponseModel>();
                foreach (var review in reviews)
                {
                    reviewDetails.Add(new ReviewResponseModel
                    {
                        UserId = review.UserId,
                        MovieId = review.MovieId,
                        Rating = review.Rating,
                        ReviewText = review.ReviewText
                    });
                }
                return reviewDetails;
        }

        public async Task< IEnumerable<MovieCardResponseModel>> GetTop25GrossingMovies()
        {
            var movies = await _movieRepository.GetTopRevenueMovies();
            var movieCardResponseModel = new List<MovieCardResponseModel>();
            foreach (var movie in movies)
            {
                var movieCard = new MovieCardResponseModel
                {
                    Id = movie.Id,
                    PosterUrl = movie.PosterUrl,
                    Revenue = movie.Revenue,
                    Title = movie.Title
                };
                movieCardResponseModel.Add(movieCard);

            }

            return movieCardResponseModel;
        }
        public async Task<List<MovieDetailsResponseModel>> GetAllMovies(MovieParameters movieParameters)
        {
            var movieDetailsResponse = new List<MovieDetailsResponseModel>();
            var movies = await _movieRepository.GetMovies(movieParameters);
            foreach (var movie in movies)
            {
                var movieDetails = new MovieDetailsResponseModel
                {
                    Id = movie.Id,
                    Title = movie.Title,
                    Overview = movie.Overview,
                    Tagline = movie.Tagline,
                    Budget = movie.Budget,
                    Revenue = movie.Revenue,
                    ImdbUrl = movie.ImdbUrl,
                    TmdbUrl = movie.TmdbUrl,
                    PosterUrl = movie.PosterUrl,
                    BackdropUrl = movie.BackdropUrl,
                    OriginalLanguage = movie.OriginalLanguage,
                    ReleaseDate = movie.ReleaseDate,
                    RunTime = movie.RunTime,
                    Price = movie.Price,
                    Genres = new List<GenreModel>(),
                    Casts = new List<CastResponseModel>()
                };
                foreach (var genre in movie.Genres)
                {
                    movieDetails.Genres.Add(new GenreModel { Id = genre.Id, Name = genre.Name });
                }
                foreach (var cast in movie.MovieCasts)
                {
                    movieDetails.Casts.Add(new CastResponseModel
                    {
                        Id = cast.CastId,
                        Character = cast.Character,
                        Name = cast.Cast.Name,
                        ProfilePath = cast.Cast.ProfilePath
                    });
                }
                movieDetailsResponse.Add(movieDetails);


            }

            return movieDetailsResponse;
        }


        public async Task<List<MovieDetailsResponseModel>> GetAllMovies()
        {
            var movieDetailsResponse = new List<MovieDetailsResponseModel>();
            var movies = await _movieRepository.GetMovies();

            foreach (var movie in movies)
            {
                var movieDetails = new MovieDetailsResponseModel
                {
                    Id = movie.Id,
                    PosterUrl = movie.PosterUrl,
                    Revenue = movie.Revenue,
                    Title = movie.Title,
                    OriginalLanguage = movie.OriginalLanguage,
                    ReleaseDate = movie.ReleaseDate,
                    RunTime = movie.RunTime,
                    Price = movie.Price
                };
                movieDetailsResponse.Add(movieDetails);

            }

            return movieDetailsResponse;




/*            return movieDetails;*/
        }

        public async Task<List<MovieRatingResponseModel>> GetTop10RatedMovies()
        {
            var movies = await _movieRepository.GetTopRatedMovies();

            var movieResponse = new List<MovieRatingResponseModel>();
            foreach (var movie in movies)
            {
                movieResponse.Add(new MovieRatingResponseModel
                {
                    Id = movie.Id,
                    PosterUrl = movie.PosterUrl,
                    Title = movie.Title,
                    ReleaseDate = movie.ReleaseDate,
                    Rating = movie.Rating
                });
            }
            return movieResponse;
        }

        public async Task<PaginatedList<BasicMovieResponseModel>> GetMoviesByGenre(int genreId, int pageSize = 30,
            int page = 1)
        {
            var pagedMovies = await _movieRepository.GetMoviesByGenre(genreId, pageSize, page);
            var data = _mapper.Map<PaginatedList<BasicMovieResponseModel>>(pagedMovies);
            var movies = new PaginatedList<BasicMovieResponseModel>(data, pagedMovies.TotalCount, page, pageSize);
            return movies;
        }

        public async Task<PagedResultSet<BasicMovieResponseModel>> GetAllMoviePurchasesByPagination(int pageSize = 50,
            int page = 0)
        {
            var totalPurchases = await _purchaseRepository.GetCountAsync();
            var purchases = await _purchaseRepository.GetAllPurchases(pageSize, page);

            var data = _mapper.Map<List<BasicMovieResponseModel>>(purchases);
            var purchasedMovies = new PagedResultSet<BasicMovieResponseModel>(data, page, pageSize, totalPurchases);
            return purchasedMovies;
        }

        public async Task<BasicCastModel> GetCastById(int id)
        {

            var dbCast = await _movieRepository.GetCastById(id);
            var movieCast = new BasicCastModel();
            movieCast.Id = dbCast.Id;
            movieCast.Gender = dbCast.Gender;
            movieCast.Name = dbCast.Name;
            movieCast.ProfilePath = dbCast.ProfilePath;
            movieCast.TmdbUrl = dbCast.TmdbUrl;

            /*foreach (var cast in dbCast)
                movieCast.Add(new CastResponseModel
                {
                    Id = cast.Id,
                    Gender = cast.Gender,
                    Name = cast.Name,
                    ProfilePath = cast.ProfilePath,
                    TmdbUrl = cast.TmdbUrl,
                });*/

            return movieCast;
        }
    }
}