// Code written by Gabriel Mailhot, 07/10/2020.

#region

using System;

#endregion

namespace TalesDAL
{
    public static class TalesRandom
    {
        private static readonly object SyncObj = new object();
        private static Random _random;


        public static bool EvalPercentage(float value)
        {
            var r = GenerateRandomNumber(100);

            return r < value;
        }

        public static bool EvalPercentageRange(int min, int max)
        {
            return EvalPercentage(GenerateRandomNumber(min, max));
        }

        public static int GenerateRandomNumber(int max)
        {
            InitRandomNumber(Guid.NewGuid().GetHashCode());

            var n = max;
            if (n < 0) n = -n;

            lock (SyncObj)
            {
                if (_random == null) _random = new Random();

                return max < 0
                    ? -_random.Next(n)
                    : _random.Next(n);
            }
        }

        public static int GenerateRandomNumber(int min, int max)
        {
            var m = min;
            var n = max;

            if (m < 0 && n >= 0) throw new ApplicationException("Error trying to generate random number; min and max must be either negatives or positives.  This mod doesn't support randomize between negative min and positive max.");

            if (m < 0) m = -m;
            if (n < 0) n = -n;

            if (m > n)
            {
                var t = n;
                n = m;
                m = t;
            }

            lock (SyncObj)
            {
                if (_random == null) _random = new Random();

                return max < 0
                    ? -_random.Next(m, n)
                    : _random.Next(m, n);
            }
        }

        public static void InitRandomNumber(int seed)
        {
            lock (SyncObj)
            {
                _random = new Random(seed);
            }
        }
    }
}