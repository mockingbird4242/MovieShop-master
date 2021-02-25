using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MovieShop.Core.Models.Request;
using MovieShop.Core.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MovieShop.MVC.Controllers
{
    public class AdminController : Controller
    {
        private readonly IMovieService _movieService;
        public AdminController(IMovieService movieService)
        {
            _movieService = movieService;
        }

       [Authorize(Roles ="Admin")]
       [HttpGet]
       public async Task<IActionResult> CreateMovie()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateMovie(MovieDetailsRequestModel movieDetailsRequestModel)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            // only when every validaiton passes make sure you save to database
            // call our User Service to save to Db
            var createdMovie= await _movieService.CreateMovie(movieDetailsRequestModel);
            if (createdMovie)
            {
                return Ok();
            }
            return View();
        }
    }
}
