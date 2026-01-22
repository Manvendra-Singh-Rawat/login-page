using MediatR;

namespace LoginPage.Features.Command
{
    public class AddUsernameCommand : IRequest<bool>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}
