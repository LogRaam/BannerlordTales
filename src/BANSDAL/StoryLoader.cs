// Code written by Gabriel Mailhot, 11/09/2020.

#region

using System.Collections.Generic;
using System.IO;

#endregion

namespace TalesDAL
{
    #region

    #endregion

    public class StoryLoader
    {
        public void ImportStoriesFrom(DirectoryInfo folder)
        {
            var files = folder.GetFiles("*.txt");

            foreach (var file in files) new StoryImporter().ImportFrom(file);
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