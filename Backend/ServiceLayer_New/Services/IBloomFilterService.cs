namespace ServiceLayer_New.Services
{
    public interface IBloomFilterService
    {
        List<UInt64> GetHash(string username, long quantity, Double percentFP, long bitsRequired, int hashFuncRequired);
        byte[] MurmurHash128(byte[] receivedString);
        byte[] XxHash128(byte[] receivedString);
    }
}
