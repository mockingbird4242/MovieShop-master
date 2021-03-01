using ApplicationCore.Exceptions;
using AutoMapper;
using MovieShop.Core.Entities;
using MovieShop.Core.Exceptions;
using MovieShop.Core.Models.Request;
using MovieShop.Core.Models.Response;
using MovieShop.Core.RepositoryInterfaces;
using MovieShop.Core.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace MovieShop.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ICryptoService _cryptoService;
        private readonly IPurchaseRepository _purchaseRepository;
        private readonly IMapper _mapper;
        private readonly ICurrentLogedInUser _currentLogedInUser;
        private readonly IAsyncRepository<Favorite> _favoriteRepository;
        public UserService(IUserRepository userRepository, ICryptoService cryptoService, IPurchaseRepository purchaseRepository, IMapper mapper, ICurrentLogedInUser currentLogedInUser, IAsyncRepository<Favorite> favoriteRepository)
        {
            _userRepository = userRepository;
            _cryptoService = cryptoService;
            _purchaseRepository = purchaseRepository;
            _mapper = mapper;
            _currentLogedInUser = currentLogedInUser;
            _favoriteRepository = favoriteRepository;
        }

        public async Task<User> GetUser(string email)
        {
            return await _userRepository.GetUserByEmail(email);
        }

        public async Task<UserRegisterResponseModel> GetUserById(int id)
        {
            var user = await _userRepository.GetByIdAsync(id);
           

            var response = _mapper.Map<UserRegisterResponseModel>(user);
            return response;
        }

        public async Task<bool> PurchaseMovie(PurchaseRequestModel purchaseRequest)
        {
            decimal? totalPrice = await _purchaseRepository.GetPriceByMovieId(purchaseRequest.MovieId);
            DateTime PurchaseDateTime = DateTime.Now;
            Guid PurchaseNumber = Guid.NewGuid();


            var purchaseResponse = new Purchase
            {
                MovieId = purchaseRequest.MovieId,
                PurchaseDateTime = PurchaseDateTime,
                PurchaseNumber = PurchaseNumber,
                TotalPrice = (decimal)totalPrice,
                UserId = purchaseRequest.UserId,


            };
            var createdPurchase = await _purchaseRepository.AddAsync(purchaseResponse);

            if (createdPurchase != null && createdPurchase.Id > 0)
            {
                return true;
            }
            return false;

        }

        public async Task<bool> RegisterUser(UserRegisterRequestModel userRegisterRequestModel)
        {
            // we need to check whether that email exists or not
            var dbUser = await _userRepository.GetUserByEmail(userRegisterRequestModel.Email);
            if (dbUser != null)
            {
                throw new ConflictException("Email already exists");
            }

            // first generate Salt

            var salt = _cryptoService.GenerateRandomSalt();
            var hashedPassword = _cryptoService.HashPassword(userRegisterRequestModel.Password, salt);
            // hash the password with salt and save the salt and hashed password to the Database

            var user = new User
            {
                Email = userRegisterRequestModel.Email,
                Salt = salt,
                HashedPassword = hashedPassword,
                FirstName = userRegisterRequestModel.FirstName,
                LastName = userRegisterRequestModel.LastName,
                DateOfBirth = userRegisterRequestModel.DateOfBirth,
                Roles = new List<Role>()


            };


            user.Roles.Add(new Role {Name = userRegisterRequestModel.Role });


            /*user.Roles.Add(new Role { Name = userRegisterRequestModel.Role, Users = new List<User>((IEnumerable<User>)user)});*/



            var createdUser = await _userRepository.AddAsync(user);

            if (createdUser != null && createdUser.Id > 0)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseModel> ValidateUser(LoginRequestModel loginRequestModel)
        {
            var dbUser = await _userRepository.GetUserByEmail(loginRequestModel.Email);

            if (dbUser == null)
            {
                return null;
            }

            var hashedPassword = _cryptoService.HashPassword(loginRequestModel.Password, dbUser.Salt);

            // get the roles of that user and 


            if (hashedPassword == dbUser.HashedPassword)
            {
                // User has entered correct password

                var loginResponse = new LoginResponseModel
                {
                    Id = dbUser.Id,
                    Email = dbUser.Email,
                    FirstName = dbUser.FirstName,
                    LastName = dbUser.LastName,
                    DateOfBirth = dbUser.DateOfBirth,
                    Roles = new List<RoleModel>()

                };
                foreach (var role in dbUser.Roles)
                {
                    
                    loginResponse.Roles.Add(new RoleModel { Id = role.Id, Name = role.Name });
                }

                return loginResponse;
            }

            return null;

        }
        public async Task AddFavorite(FavoriteRequestModel favoriteRequest)
        {
            if (_currentLogedInUser.UserId != favoriteRequest.UserId)
                throw new HttpException(HttpStatusCode.Unauthorized, "You are not Authorized to Favorite");
            // See if Movie is already Favorite.
            if (_currentLogedInUser.UserId != null) favoriteRequest.UserId = _currentLogedInUser.UserId.Value;
            if (await FavoriteExists(favoriteRequest.UserId, favoriteRequest.MovieId))
                throw new ConflictException("Movie already Favorited");

            var favorite = _mapper.Map<Favorite>(favoriteRequest);
            await _favoriteRepository.AddAsync(favorite);
        }
        public async Task<bool> FavoriteExists(int id, int movieId)
        {
            return await _favoriteRepository.GetExistingAsync(f => f.MovieId == movieId &&
                                                                 f.UserId == id);
        }
    }
}
