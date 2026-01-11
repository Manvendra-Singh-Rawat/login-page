using LoginPage.Entity;
using LoginPage.Features.Querys;
using LoginPage.Models;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer_New.Services;

namespace LoginPage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public ISender _sender;
        public AuthController (ISender sender) => _sender = sender;

        [HttpPost("register")]
        public ActionResult<User> Register(UserDto userDto)
        {
            User user = new User();
            var passwordHash = new PasswordHasher<User>().HashPassword(user, userDto.password);

            return user;
        }

        [HttpPost("checkAvailability:{newUsername}")]
        public async Task<ActionResult<string>> CheckAvailability(string newUsername)
        {
            await _sender.Send(new CheckUsernameQuery(newUsername));
            return Ok("Username is available");
        }

        [HttpGet("checkservices")]
        public ActionResult<string> CheckService()
        {
            return Ok("Working hehe");
        }
    }
}
