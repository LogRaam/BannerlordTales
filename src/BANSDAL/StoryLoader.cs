// Code written by Gabriel Mailhot, 29/08/2020.

namespace TalesDAL
{
   #region

   using System.Collections.Generic;
   using System.IO;

   #endregion

   public class StoryLoader
   {
      public void ImportStoriesFrom(DirectoryInfo folder)
      {
         FileInfo[] files = folder.GetFiles("*.txt");

         foreach (FileInfo file in files) new StoryImporter().ImportFrom(file);
      }

      #region private

      private List<FileInfo> RetrievePathsFrom(DirectoryInfo folder)
      {
         var result = new List<FileInfo>();

         return result;
      }

      #endregion
   }
}