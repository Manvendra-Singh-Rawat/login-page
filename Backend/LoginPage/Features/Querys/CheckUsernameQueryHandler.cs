using LoginPage.Entity;
using MediatR;
using ServiceLayer_New.Services;

namespace LoginPage.Features.Querys
{
    public class CheckUsernameQueryHandler : IRequestHandler<CheckUsernameQuery, bool>
    {
        private readonly AbuseWorkChecker check = new();
        private readonly BloomFilterService bloomFilterService = new();
        public async Task<bool> Handle(CheckUsernameQuery request, CancellationToken cancellationToken)
        {
            Console.WriteLine("Inside CheckUsernameQueryHandler handle");
            bool isAbusive = check.CheckAbuse(request.Username);

            if (isAbusive)
            {
                Console.WriteLine("is abusive damn");
                return true;
            }            

            List<UInt64> hashedList = bloomFilterService.GetHash(
                request.Username, 
                BloomFilterData.n, 
                BloomFilterData.p, 
                BloomFilterData.m, 
                BloomFilterData.k);

            // Async DB call

            return true;
        }
    }
}
