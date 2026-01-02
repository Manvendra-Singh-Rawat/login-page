namespace ServiceLayer_New.Services
{
    public interface IBloomFilterService
    {
        void GetHash(string asdf);
        byte[] MurmurHash128(byte[] receivedString);
        byte[] XxHash128(byte[] receivedString);
    }
}
