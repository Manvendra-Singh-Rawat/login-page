namespace LoginPage.Infrastructure.Persistence.DBClasses
{
    public class Users
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
    }
}
