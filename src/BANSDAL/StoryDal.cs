// Code written by Gabriel Mailhot, 02/12/2023.

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
        public List<IStory> LoadStoriesFromFile(FileInfo filePath)
        {
            var result = new List<IStory>();
            result.AddRange(new StoryImporter().ImportFrom(filePath));

            return result;
        }

        public List<IStory> LoadStoriesFromFolder(DirectoryInfo folder)
        {
            var result = new List<IStory>();
            var files = folder.GetFiles("*.txt");
            foreach (var file in files) result.AddRange(new StoryImporter().ImportFrom(file));

            return result;
        }
    }
}