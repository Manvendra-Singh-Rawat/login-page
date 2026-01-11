using System.Collections;

namespace LoginPage.Entity
{
    public sealed class BloomFilterData
    {
        // Expected quantity of names
        public readonly static long n = 100000000;
        // Percentage of false positive
        public readonly static Double p = 0.01;
        // Number of bits required
        public readonly static int m = (int)(-(n * Math.Log(p)) / Math.Log(4));
        // Number of hash functions required
        public readonly static int k = (int)((int)(-(n * Math.Log(p)) / Math.Log(4)) / n * Math.Log(2));

        public static BitArray bloomFilterArrayNew = new BitArray((int)(-(n * Math.Log(p)) / Math.Log(4)));
    }
}

//m = -(N* ln(p)) / (ln(2)^2)
//public static byte[] bloomFilterArray = new byte[(int)(-(n * Math.Log(0.01)) / Math.Log(4))];
