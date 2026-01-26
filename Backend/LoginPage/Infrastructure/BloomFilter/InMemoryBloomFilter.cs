using LoginPage.Application.Interfaces;
using System.Collections;
using Murmur;
using System.IO.Hashing;

namespace LoginPage.Infrastructure.BloomFilter
{
    public class InMemoryBloomFilter : IBloomFilterService
    {
        private readonly BitArray InMemoryBitArray;
        private readonly int _size;
        private readonly int _hashCount;

        private readonly Murmur128 _murmur128 = MurmurHash.Create128();
        private readonly XxHash128 _xxHash128 = new XxHash128();

        public InMemoryBloomFilter(IConfiguration config)
        {
            UInt64 n = config.GetValue<UInt64>("BloomFilterData:ExpectedItems");
            double p = config.GetValue<double>("BloomFilterData:FalsePositiveRate");

            if (n == 0)
                throw new Exception("ExpectedItems cannot be zero");

            if (p <= 0 || p >= 1)
                throw new Exception("FalsePositiveRate must be between 0 and 1");

            _size = (int)Math.Ceiling(-(n * Math.Log(p)) / Math.Pow(Math.Log(2), 2));
            _hashCount = (int)Math.Round((_size / (double)n) * Math.Log(2));

            InMemoryBitArray = new BitArray(_size);

            PopulateBloomFilter(config);
        }

        private void PopulateBloomFilter(IConfiguration config = null)
        {
            string? filePath = config.GetValue<string>("FilePath");
            if (filePath == null) return;

            bool isEsists = File.Exists(filePath);
            if (!isEsists)
            {
                Console.WriteLine("File path is empty!");
                return;
            }
            else
            {
                string content = File.ReadAllText(filePath);

                var newString = content.Split(' ');
                Console.WriteLine("Count: " + newString.Count());
                foreach (string line in newString)
                {
                    Console.WriteLine($"{line}");
                    if (line == "") continue;
                    InMemoryBitArray[int.Parse(line)] = true;
                }

                Console.WriteLine("File content:");
                Console.WriteLine(content);
            }
        }

        public void Add(string username)
        {
            GetULongHash(username, out ulong murmurHashULong, out ulong xxHashULong);
            List<int> combinedHashList = GetCombinedHash(murmurHashULong, xxHashULong);

            foreach (int hash in combinedHashList)
            {
                InMemoryBitArray[hash] = true;
            }
        }

        public bool MightContain(string username, out List<int> hashList)
        {
            GetULongHash(username, out ulong murmurHashULong, out ulong xxHashULong);
            List<int> combinedHashList = GetCombinedHash(murmurHashULong, xxHashULong);

            bool flag = true;
            foreach(int hash in combinedHashList)
            {
                if (InMemoryBitArray[hash] == false)
                {
                    flag = false; break;
                }
            }

            hashList = combinedHashList;

            return flag;
        }

        private void GetULongHash(string username, out ulong murmurHashULong, out ulong xxHashULong)
        {
            byte[] byteString = System.Text.ASCIIEncoding.UTF8.GetBytes(username);

            var murmurHashedBytes = _murmur128.ComputeHash(byteString);
            murmurHashULong = BitConverter.ToUInt64(murmurHashedBytes);

            _xxHash128.Reset();
            _xxHash128.Append(byteString);
            var xxHashedBytes = _xxHash128.GetCurrentHash();
            xxHashULong = BitConverter.ToUInt64(xxHashedBytes);
        }

        private List<int> GetCombinedHash(ulong hash1, ulong hash2)
        {
            List<int> combinedHashList = new List<int>();
            for (int i = 0; i < _hashCount; i++)
            {
                ulong combined = hash1 + ((ulong)i * hash2);
                int index = (int)(combined % (ulong)_size);
                combinedHashList.Add(index);
            }

            return combinedHashList;
        }
    }
}
