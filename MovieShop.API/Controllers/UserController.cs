using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieShop.Core.Models.Request;
using MovieShop.Core.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ICurrentLogedInUser _currentUser;
        public UserController(IUserService userService, IMovieService movieService, ICurrentLogedInUser currentUser)
        {
            _userService = userService;
            _currentUser = currentUser;
        }


        [Authorize]
        [HttpGet("{id:int}/purchases")]
        public async Task<ActionResult> GetUserPurchasedMoviesAsync(int id)
        {
            if (id != _currentUser.UserId.Value)
            {
                return Unauthorized("Hey you cannot access other person info");
            }

            // we ahve to check the id from the url is equal to the id from the JWT token then only show the data
            return Ok();
          //  var userMovies = await _userService.GetAllPurchasesForUser(id);
           // return Ok(userMovies);
        }


        [HttpPost("purchase")]
        public async Task<ActionResult> CreatePurchase([FromBody] PurchaseRequestModel purchaseRequest)
        {
            await _userService.PurchaseMovie(purchaseRequest);
            return Ok();
        }

        [HttpPost("favorite")]
        public async Task<ActionResult> CreateFavorite([FromBody] FavoriteRequestModel favoriteRequest)
        {
            await _userService.AddFavorite(favoriteRequest);
            return Ok();
        }
    }
}
