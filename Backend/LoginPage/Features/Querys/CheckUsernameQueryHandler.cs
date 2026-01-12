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

            foreach(UInt64 hashedHash in hashedList)
            {
                Console.WriteLine(hashedHash);
            }

            bool flag = true;
            foreach (UInt64 hashedHash in hashedList)
            {
                if (BloomFilterData.bloomFilterArrayNew[(int)hashedHash] == false)
                {
                    flag = false;
                    break;
                }
            }

            if(flag)
            {
                // returning false to show that the given username is in the DB(including false positive) so you can't use it.
                return false;
            }

            // Async DB call
            // previous filter did its job not its time for DB to actually check if the username is in db or not

            return true;
        }
    }
}
