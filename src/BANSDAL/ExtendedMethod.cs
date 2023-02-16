// unset

#region

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

#endregion

namespace TalesDAL
{
    public static class ExtendedMethod
    {
        public static string ExtractLinkWithoutPercentage(this string line)
        {
            var s = line.Split(':').RemoveEmptyItems();
            var t1 = s[1].Split(' ').RemoveEmptyItems();

            var p = "";
            for (var i = 0; i < t1.Length - 1; i++)
            {
                p = p + t1[i] + " ";
            }

            return p.Trim().Replace(".", "");
        }


        public static int ExtractPercentageChancesFromLink(this string line)
        {
            var s = line.Split(':').RemoveEmptyItems();
            var t2 = s[1].Split(' ').RemoveEmptyItems().Last().Replace("%", string.Empty);

            return Convert.ToInt32(t2);
        }

        public static bool ReferTo(this string line, string value)
        {
            var result = line.ToUpper().TrimStart().StartsWith(value);

            return result;
        }

        public static string Reformat(this string line)
        {
            var result = CultureInfo.CurrentCulture.TextInfo.ToLower(line);
            result = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(result).Trim();
            
            //var result = line.ToUpper().Trim();

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