using LoginPage.Application.Interfaces;
using LoginPage.Entity;
using LoginPage.Infrastructure.Persistence;
using LoginPage.Infrastructure.Persistence.DBClasses;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace LoginPage.Features.Command
{
    public class AddUsernameCommandHandler : IRequestHandler<AddUsernameCommand, User>
    {
        private readonly IAbuseCheckerService abuseCheckerService;
        private readonly IBloomFilterService bloomFilterService;

        private readonly PostgresDbContext _postgresDbContext;

        private readonly IFileService fileService;

        private static User newUser = new User();

        public AddUsernameCommandHandler(IAbuseCheckerService abuseService, IBloomFilterService bloomService, PostgresDbContext dbContext, IFileService fileService)
        {
            abuseCheckerService = abuseService;
            bloomFilterService = bloomService;
            _postgresDbContext = dbContext;
            this.fileService = fileService;
        }

        public async Task<User> Handle(AddUsernameCommand request, CancellationToken cancellationToken)
        {
            bool isAbusive = abuseCheckerService.Check(request.Username);
            
            if (isAbusive) return null;

            bool mightContain = bloomFilterService.MightContain(request.Username, out List<int> data);

            if (mightContain)
            {
                Console.WriteLine("username already taken! (bloom filter)");
                return null;
            }

            // DB check
            bool existsInDb = await _postgresDbContext.Users.AnyAsync(x => x.Username == request.Username, cancellationToken);
            
            if(existsInDb)
            {
                Console.WriteLine("Username already taken!");
                return null;
            }
            else
            {
                Console.WriteLine("Username doesn't exist in DB");
            }

            bloomFilterService.Add(request.Username);

            var hashedPassword = new PasswordHasher<User>().HashPassword(newUser, request.Password);

            await _postgresDbContext.Users.AddAsync(new Users
            {
                Username = request.Username,
                PasswordHash = hashedPassword
            }, cancellationToken);

            await _postgresDbContext.SaveChangesAsync();

            string hashString = "";
            foreach(var newdata in data)
            {
                hashString += newdata.ToString() + " ";
            }

            _ = Task.Run(async () =>
            {
                try
                {
                    await fileService.WriteToFileAsync("E:\\Projects\\login-page\\Backend\\LoginPage\\Infrastructure\\FileService\\BloomFilterIndexes.txt", hashString);
                }
                catch (Exception ex)
                {
                    // log it
                    Console.Write(ex.ToString());
                }
            });

            // here null is just placeholder
            // will switch to a user class for return to fix this
            return newUser;
        }

        private async void AddDataToFile(string data)
        {
            await fileService.WriteToFileAsync("E:\\Projects\\login-page\\Backend\\LoginPage\\Infrastructure\\FileService\\BloomFilterIndexes.txt", data);
        }
    }
}
