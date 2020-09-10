// Code written by Gabriel Mailhot, 29/08/2020.

namespace TalesContract
{
   #region

   using System.Collections.Generic;
   using System.IO;

   #endregion

   public interface IDAL
   {
      List<IStory> LoadStoriesFromFolder(DirectoryInfo moduleFolder);
   }
}