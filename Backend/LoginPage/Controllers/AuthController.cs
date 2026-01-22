using LoginPage.Entity;
using LoginPage.Features.Command;
using LoginPage.Features.Querys;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LoginPage.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public ISender _sender;
        public AuthController (ISender sender) => _sender = sender;

        [HttpPost("register")]
        public async Task<ActionResult<User>> RegisterAsync(AddUsernameCommand command)
        {
            var update = await _sender.Send(command);
            return null;
        }

        [HttpPost("checkAvailability:{newUsername}")]
        public async Task<ActionResult<string>> CheckAvailability(string newUsername)
        {
            bool isAvailable = await _sender.Send(new CheckUsernameQuery(newUsername));
            
            return Ok(new
                {
                    isAvailable,
                    message = isAvailable ? "Username is available" : "Username is not available"
                }
            );
        }
    }
}
