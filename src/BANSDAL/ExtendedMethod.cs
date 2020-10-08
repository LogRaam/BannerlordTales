// Code written by Gabriel Mailhot, 06/10/2020.

#region

using System.Collections.Generic;

#endregion

namespace TalesDAL
{
    public static class ExtendedMethod
    {
        public static bool ReferTo(this string line, string value)
        {
            var result = line.ToUpper().TrimStart().StartsWith(value);

            return result;
        }

        public static string Reformat(this string line)
        {
            var result = line.ToUpper().Trim();

            return result;
        }

        public static List<string> RemoveEmptyItems(this List<string> array)
        {
            array.RemoveAll(n => n == string.Empty);

            return array;
        }

        public static string[] RemoveEmptyItems(this string[] array)
        {
            var result = new List<string>();

            foreach (var t in array)
            {
                if (string.IsNullOrEmpty(t)) continue;
                if (string.IsNullOrWhiteSpace(t)) continue;

                result.Add(t);
            }

            return result.ToArray();
        }

        public static string[] RemoveSpecialCharacters(this string[] array)
        {
            var result = new string[array.Length];

            for (var i = 0; i < array.Length; i++)
            {
                var n = array[i];
                n = n.Replace("\t", "");
                n = n.Replace("\n", "");
                n = n.Replace("\r", "");
                result[i] = n;
            }


            return result;
        }
    }
}