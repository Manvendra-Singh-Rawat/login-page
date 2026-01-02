using Murmur;
using System.IO.Hashing;

namespace ServiceLayer_New.Services
{
    public class BloomFilterService : IBloomFilterService
    {
        private static readonly Murmur128 murmurHash128 = MurmurHash.Create128();
        private static readonly XxHash128 xxHash128 = new XxHash128();

        public void GetHash(string asdf)
        {
            byte[] byteString = System.Text.ASCIIEncoding.UTF8.GetBytes(asdf);

            byte[] murmurHashedBytes = MurmurHash128(byteString);
            byte[] xxHashedBytes = XxHash128(byteString);

            GenerateNewHash(murmurHashedBytes, xxHashedBytes);
        }

        public byte[] MurmurHash128(byte[] receivedString)
        {
            byte[] murmurHashedString = murmurHash128.ComputeHash(receivedString);
            Console.Write("\nMurmur hash: ");
            foreach(var ss in murmurHashedString) Console.Write(ss.ToString() + " ");
            return murmurHashedString;
        }

        public byte[] XxHash128(byte[] receivedString)
        {
            xxHash128.Reset();
            xxHash128.Append(receivedString);
            byte[] xxHashedString = xxHash128.GetCurrentHash();
            Console.Write("\nXxHash128 hash: ");
            foreach (var ss in xxHashedString) Console.Write(ss.ToString() + " ");
            return xxHashedString;
        }

        private byte[] GenerateNewHash(byte[] byteArray1, byte[] byteArray2)
        {
            byte[] result = new byte[byteArray1.Length];
            for(int i = 0; i < byteArray1.Length; i++)
            {

            }

            return result;
        }
    }
}
