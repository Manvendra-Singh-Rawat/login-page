using MediatR;

namespace LoginPage.Features.Querys
{
    public class CheckUsernameQuery : IRequest<bool>
    {
        public string Username { get; set; } = string.Empty;

        public CheckUsernameQuery(string username)
        {
            Username = username;
        }
    }
}
