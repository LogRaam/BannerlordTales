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


        public static bool EvalPercentage(int value)
        {
            InitRandomNumber(Guid.NewGuid().GetHashCode());

            return GenerateRandomNumber(100) < value;
        }

        public static bool EvalPercentageRange(int min, int max)
        {
            return EvalPercentage(GenerateRandomNumber(min, max));
        }

        public static int GenerateRandomNumber(int max)
        {
            lock (SyncObj)
            {
                if (_random == null) _random = new Random(); // Or exception...

                return _random.Next(max);
            }
        }

        public static int GenerateRandomNumber(int min, int max)
        {
            lock (SyncObj)
            {
                if (_random == null) _random = new Random(); // Or exception...

                return _random.Next(min, max);
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