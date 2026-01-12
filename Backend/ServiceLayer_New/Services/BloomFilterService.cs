using Murmur;
using System.IO.Hashing;

namespace ServiceLayer_New.Services
{
    public class BloomFilterService : IBloomFilterService
    {
        private static readonly Murmur128 murmurHash128 = MurmurHash.Create128();
        private static readonly XxHash128 xxHash128 = new XxHash128();

        // Expected quantity of names
        public long n;
        // Percentage of false positive
        public Double p;
        // Number of bits required
        public long m;
        // Number of hash functions required
        public int k;

        public List<UInt64> GetHash(string username, long quantity, double percentFP, long bitsRequired, int hashFuncRequired)
        {
            n = quantity;
            p = percentFP;
            m = bitsRequired;
            k = hashFuncRequired;
            Console.WriteLine(n);
            Console.WriteLine(k);

            byte[] byteString = System.Text.ASCIIEncoding.UTF8.GetBytes(username);

            byte[] murmurHashedBytes = MurmurHash128(byteString);
            byte[] xxHashedBytes = XxHash128(byteString);

            ulong murmurHash64 = BitConverter.ToUInt64(murmurHashedBytes.AsSpan()[..16]);
            ulong xxHash64 = BitConverter.ToUInt64(xxHashedBytes.AsSpan()[..16]);

            GenerateNewHash(murmurHash64, xxHash64, out List<ulong> hashedList);

            Console.WriteLine($"hashedList count: {hashedList.Count}");

            return hashedList;
        }

        public byte[] MurmurHash128(byte[] receivedString)
        {
            byte[] murmurHashedString = murmurHash128.ComputeHash(receivedString);
            return murmurHashedString;
        }

        public byte[] XxHash128(byte[] receivedString)
        {
            xxHash128.Reset();
            xxHash128.Append(receivedString);
            byte[] xxHashedString = xxHash128.GetCurrentHash();
            return xxHashedString;
        }

        private void GenerateNewHash(ulong hash1, ulong hash2, out List<ulong> result)
        {
            result = new List<ulong>(k);
            ulong mod = (ulong)m;

            for (ulong i = 0; i < (ulong)k; i++)
            {
                ulong index = (hash1 + i * hash2) % mod;
                result.Add(index);
            }
        }
    }
}
