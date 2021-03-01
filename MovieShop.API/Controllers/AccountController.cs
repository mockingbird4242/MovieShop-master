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
        private readonly IJwtService _jwtService;
        public AccountController(IUserService userService, IJwtService jwtService)
        {
            _userService = userService;
            _jwtService = jwtService;
        }
        [HttpPost]
        public async Task<IActionResult> RegisterUser([FromBody]UserRegisterRequestModel requestModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Please check data");
            }
            bool registeredUser = await _userService.RegisterUser(requestModel);
            return Ok(registeredUser);
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> GetUserByID(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user != null)
            {
                return Ok(user);
            }
            return NotFound("No User Found");
        }

        [HttpPost("login")]
        public async Task<ActionResult> LoginAsync([FromBody] LoginRequestModel loginRequest)
        {
            var user = await _userService.ValidateUser(loginRequest);
            if (user == null) return Unauthorized();

            var tokenObject = new { token = _jwtService.GenerateJWT(user) };
            return Ok(tokenObject  );
        }

        [HttpGet]
        public async Task<ActionResult> EmailExists([FromQuery] string email)
        {
            var user = await _userService.GetUser(email);
            return Ok(user == null ? new { emailExists = false } : new { emailExists = true });
        }


    }
}
