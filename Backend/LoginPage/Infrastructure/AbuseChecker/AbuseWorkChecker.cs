using LoginPage.Application.Interfaces;

namespace LoginPage.Infrastructure.AbuseChecker
{
    public class AbuseWorkChecker : IAbuseCheckerService
    {
        private readonly List<string> abuseWords = new List<string>()
        {
            "nigger","niger","asian","chinki",
            "ass","pussy","dick","d1ck","dik","d1k","boob","boobs",
            "s3x","sex","69","619"
        };

        public bool Check(string username)
        {
            foreach (string s in abuseWords)
            {
                if (username.Contains(s)) return true;
            }
            return false;
        }
    }
}
