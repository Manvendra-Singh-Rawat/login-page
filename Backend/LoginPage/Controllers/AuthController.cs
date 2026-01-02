using LoginPage.Entity;
using LoginPage.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using ServiceLayer_New.Services;

namespace LoginPage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        BloomFilterService newBloomFilterService = new BloomFilterService();
        AbuseWorkChecker abuseWorkChecker = new AbuseWorkChecker();

        [HttpPost("register")]
        public ActionResult<User> Register(UserDto userDto)
        {
            User user = new User();
            var passwordHash = new PasswordHasher<User>().HashPassword(user, userDto.password);

            return user;
        }

        [HttpPost("checkAvailability")]
        public ActionResult<string> CheckAvailability(UsernameDto usernameDto)
        {
            // Abuse word checker
            // check if the a list of string is not in the username
            if(abuseWorkChecker.CheckAbuse(usernameDto.username))
            {
                return BadRequest("bad username");
            }

            newBloomFilterService.GetHash(usernameDto.username);
            return Ok("Username is available");
        }

        [HttpGet("CheckServiceWorking")]
        public ActionResult<string> CheckService()
        {
            return Ok(true);
        }
    }
}
