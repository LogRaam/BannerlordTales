// Code written by Gabriel Mailhot, 11/09/2020.

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
            var result = new List<IStory>();
            var files = folder.GetFiles("*.txt");
            foreach (var file in files) result.Add(new StoryImporter().ImportFrom(file));

            return result;
        }
    }
}