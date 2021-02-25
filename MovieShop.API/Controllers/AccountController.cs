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
    public class AccountController : ControllerBase
    {
        /*private readonly IUserService _userService;
        [HttpPost]
        public async Task<IActionResult> RegisterUser(UserRegisterRequestModel requestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please check data");
            }
            var registeredUser = await _userService.RegisterUser(requestModel);
            return Ok(registeredUser);
        }*/
        private readonly IUserService _userService;
        public AccountController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost]
        public async Task<IActionResult> RegisterUser(UserRegisterRequestModel requestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please check data");
            }
            bool registeredUser = await _userService.RegisterUser(requestModel);
            return Ok(registeredUser);
        }

    }
}
