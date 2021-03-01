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
    public class AdminController : ControllerBase
    {
        private readonly IMovieService _movieService;
        public AdminController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [HttpPost("movie")]
        public async Task<IActionResult> CreateMovie([FromBody] MovieDetailsRequestModel movieDetailsRequestModel)
        {
            var createdMovie = await _movieService.CreateMovie(movieDetailsRequestModel);
            return Ok(createdMovie);

        }

        [HttpPut("movie/{id:int}")]
        public async Task<IActionResult> UpdateMovie([FromRoute] int id, [FromBody] MovieDetailsRequestModel movieDetailsRequestModel)
        {
            var createdMovie = await _movieService.UpdateMovie(id, movieDetailsRequestModel);
            return Ok(createdMovie);
        }

        [HttpGet("purchases")]
        public async Task<IActionResult> GetAllPurchases([FromQuery] int pageSize = 30, [FromQuery] int page = 1)
        {
            var movies = await _movieService.GetAllMoviePurchasesByPagination(pageSize, page);
            return Ok(movies);
        }

    }

}
