// Code written by Gabriel Mailhot, 29/08/2020.

#region

using System.Collections.Generic;
using System.Linq;

#endregion

namespace TalesDAL
{
   public static class ExtendedMethod
   {
      public static bool ReferTo(this string line, string value)
      {
         bool result = line.ToUpper().TrimStart().StartsWith(value);

         return result;
      }

      public static string Reformat(this string line)
      {
         string result = line.ToUpper().Trim();

         return result;
      }

      public static List<string> RemoveEmptyItems(this List<string> array)
      {
         array.RemoveAll(n => n == string.Empty);

         return array;
      }

      public static string[] RemoveEmptyItems(this string[] array)
      {
         return array.Where(s => s != string.Empty).ToArray();
      }
   }
}