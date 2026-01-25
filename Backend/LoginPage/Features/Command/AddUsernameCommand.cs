using LoginPage.Entity;
using MediatR;

namespace LoginPage.Features.Command
{
    public class AddUsernameCommand : IRequest<User>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
