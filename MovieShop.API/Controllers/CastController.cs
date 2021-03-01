using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MovieShop.Core.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieShop.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CastController : ControllerBase
    {
        private readonly IMovieService _movieService;
        public CastController(IMovieService movieService)
        {
            _movieService = movieService;
        }
        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetCastById(int id)
        {
            var cast = await _movieService.GetCastById(id);
            return Ok(cast);

        }
    }
}
