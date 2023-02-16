// Code written by Gabriel Mailhot, 11/13/2022.

#region

using System.Collections.Generic;
using System.IO;

#endregion

namespace TalesContract
{
    #region

    #endregion

    public interface IDal
    {
        List<IStory> LoadStoriesFromFolder(DirectoryInfo moduleFolder);
    }
}