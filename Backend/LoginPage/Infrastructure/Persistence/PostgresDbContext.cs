using LoginPage.Infrastructure.Persistence.DBClasses;
using Microsoft.EntityFrameworkCore;

namespace LoginPage.Infrastructure.Persistence
{
    public class PostgresDbContext : DbContext
    {
        public PostgresDbContext (DbContextOptions<PostgresDbContext> options) : base(options)
        {
        }

        public DbSet<Users> Users { get; set; }
    }
}
