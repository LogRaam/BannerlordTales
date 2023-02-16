// Code written by Gabriel Mailhot, 02/13/2023.

#region

using System;
using System.IO;
using System.Linq;
using TaleWorlds.ModuleManager;

#endregion

namespace TalesDAL
{
    public class StoryLoader
    {
        public DirectoryInfo GetCustomStoriesDirectoryInfo()
        {
            var t = GetModuleDirectoryInfo();
            var result = new DirectoryInfo(t + "\\CustomStories");

            return result;
        }

        public DirectoryInfo GetModuleDirectoryInfo()
        {
            DirectoryInfo result;

            try
            {
                var p = ModuleHelper.GetModules().FirstOrDefault(n => n.Id == "BannerlordTales");
                result = new DirectoryInfo(ModuleHelper.GetPath(p.Id));
            }
            catch (Exception e)
            {
                result = new DirectoryInfo("P:\\OneDrive\\Programmation\\Bannerlord\\BannerlordTales");
            }

            return result;
        }

        public DirectoryInfo GetStoryImagesDirectoryInfo()
        {
            var t = GetModuleDirectoryInfo();
            var result = new DirectoryInfo(t + "\\StoryImages");

            return result;
        }
    }
}