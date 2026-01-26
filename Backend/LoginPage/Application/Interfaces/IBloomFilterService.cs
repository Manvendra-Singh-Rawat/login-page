namespace LoginPage.Application.Interfaces
{
    public interface IBloomFilterService
    {
        void Add(string username);
        bool MightContain(string username, out List<int> hashList);
    }
}
