// Code written by Gabriel Mailhot, 02/09/2020.

#region

using System.Collections.Generic;
using System.IO;
using TalesContract;
using TalesDAL;
using TaleWorlds.Library;

#endregion

namespace TalesPersistence
{
   #region

   #endregion

   public class StoryContext
   {
      private DirectoryInfo _customStoriesFolder;
      private DirectoryInfo _moduleFolder;
      private DirectoryInfo _storyImageFiles;

      public DirectoryInfo CustomStoriesFolder
      {
         get
         {
            if (GameData.Instance.GameContext.CurrentGameStarted()) _customStoriesFolder = new DirectoryInfo(ModuleFolder.FullName + "CustomStories");

            return _customStoriesFolder;
         }
         set => _customStoriesFolder = value;
      }

      public DirectoryInfo ModuleFolder
      {
         get
         {
            if (GameData.Instance.GameContext.CurrentGameStarted()) _moduleFolder = new DirectoryInfo(BasePath.Name + "Modules/LogRaamBannerlordTales");

            return _moduleFolder;
         }
         set => _moduleFolder = value;
      }

      public List<IStory> PlayedStories { get; set; } = new List<IStory>(); // TODO: should import sync data

      public List<IStory> Stories { get; set; } = new List<IStory>();

      public List<FileInfo> StoryImageFiles { get; set; } = new List<FileInfo>();

      public DirectoryInfo StoryImagesFolder
      {
         get
         {
            if (GameData.Instance.GameContext.CurrentGameStarted()) _storyImageFiles = new DirectoryInfo(ModuleFolder.FullName + "StoryImages");

            return _storyImageFiles;
         }
         set => _storyImageFiles = value;
      }

      public List<IStory> ImportStoriesFromDisk()
      {
         if (ModuleFolder == null) return new List<IStory>();

         return new StoryDal().LoadStoriesFromFolder(CustomStoriesFolder);
      }
   }
}