using CRUDinNETCORE.Helpers;
using CRUDinNETCORE.Models;
using CRUDinNETCORE.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.Contracts;

namespace CRUDinNETCORE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IUserService _userService;

        public UserController(IUserService userService) 
        {
            _userService = userService;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Get() 
        {
            return Ok(await _userService.GetAll());
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] User userObj) 
        {
            userObj.Id = 0;
            return Ok(await _userService.AddandUpdateUser(userObj));
        }

        //PUT api/<customercontroller>/5
        [HttpPut("{id}")]
        [Authorize]
        public  async Task<IActionResult> Put(int id, [FromBody] User userObj) 
        {
            userObj.Id = id;
            return Ok(await _userService.AddandUpdateUser(userObj));
        }

        [HttpPost("login")]
        public async Task<IActionResult> login(AuthenticateRequest model) 
        {
            var response = await _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = "Username or Password is incorrect " });

            return Ok(response);
        }

    }
}
