namespace ServiceLayer_New.Services
{
    public class AbuseWorkChecker()
    {
        private static readonly List<string> abuseWords = new List<string>()
        {
            "nigger","niger","asian","chinki",
            "ass","pussy","dick","d1ck","dik","d1k","boob","boobs",
            "s3x","sex","69","619"
        };

        public bool CheckAbuse(string word)
        {
            foreach(string s in abuseWords)
            {
                if(word.Contains(s)) return true;
            }
            return false;
        }
    }
}
