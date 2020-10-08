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

        public static int GenerateRandomNumber(int max)
        {
            lock (SyncObj)
            {
                if (_random == null) _random = new Random(); // Or exception...

                return _random.Next(max);
            }
        }

        public static void InitRandomNumber(int seed)
        {
            _random = new Random(seed);
        }
    }
}