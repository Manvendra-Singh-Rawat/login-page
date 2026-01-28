using LoginPage.Application.Interfaces;
using LoginPage.Infrastructure.Persistence;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LoginPage.Features.Querys
{
    public class CheckUsernameQueryHandler : IRequestHandler<CheckUsernameQuery, bool>
    {
        private readonly IBloomFilterService _bloomFilterService;
        private readonly IAbuseCheckerService _abuseCheckerService;

        private readonly PostgresDbContext _postgresDbContext;

        public CheckUsernameQueryHandler(IBloomFilterService bloomFilterService, IAbuseCheckerService abuseCheckerService, PostgresDbContext dbContext)
        {
            _bloomFilterService = bloomFilterService;
            _abuseCheckerService = abuseCheckerService;

            _postgresDbContext = dbContext;
        }

        public async Task<bool> Handle(CheckUsernameQuery request, CancellationToken cancellationToken)
        {
            bool isAbusive = _abuseCheckerService.Check(request.Username);
            if (isAbusive)
            {
                return false;
            }
            else
            {
                Console.WriteLine("Username not abusive");
            }

            bool mightContain = _bloomFilterService.MightContain(request.Username, out List<int> _);

            if(mightContain)
            {
                Console.WriteLine("username already taken!");
                return false;
            }

            bool existsInDb = await _postgresDbContext.Users.AnyAsync(x => x.Username == request.Username, cancellationToken);

            if (existsInDb)
            {
                Console.WriteLine("Username already taken!");
                return false;
            }

            return true;
        }
    }
}
