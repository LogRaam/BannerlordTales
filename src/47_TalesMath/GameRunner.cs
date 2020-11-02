// Code written by Gabriel Mailhot, 29/10/2020.

#region

using System;

#endregion

namespace _47_TalesMath
{
    public class GameRunner
    {
        public static T Runner<T>(Func<T> func)
        {
            GameFunction.Log(func.ToString());

            return func();
        }
    }
}