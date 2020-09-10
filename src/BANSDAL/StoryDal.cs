// Code written by Gabriel Mailhot, 29/08/2020.

#region

using System.Collections.Generic;
using System.IO;
using TalesContract;

#endregion

namespace TalesDAL
{
   #region

   #endregion

   public class StoryDal
   {
      public List<IStory> LoadStoriesFromFolder(DirectoryInfo folder)
      {
         List<IStory> result = new List<IStory>();
         FileInfo[] files = folder.GetFiles("*.txt");
         foreach (FileInfo file in files)
         {
            result.Add(new StoryImporter().ImportFrom(file));
         }

         return result;
      }
   }
}