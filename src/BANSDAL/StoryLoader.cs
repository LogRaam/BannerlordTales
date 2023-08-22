// Code written by Gabriel Mailhot, 02/13/2023.

#region

using _45_TalesGameState;
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

            if (CampaignState.CurrentGameStarted())
            {
                var m = ModuleHelper.GetModules().First(n => n.Id == "LogRaamBannerlordTales");
                result = new DirectoryInfo(ModuleHelper.GetPath(m.Id).Replace("\\SubModule.xml", ""));

                return result;
            }

            //Game is not running
            result = new DirectoryInfo("P:\\OneDrive\\Programmation\\Bannerlord\\BannerlordTales");

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