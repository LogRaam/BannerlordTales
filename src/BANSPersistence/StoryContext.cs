// Code written by Gabriel Mailhot, 11/09/2020.

#region

using System.Collections.Generic;
using System.IO;
using _45_TalesGameState;
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

        public Textures BackgroundImages { get; set; } = new Textures();

        public DirectoryInfo CustomStoriesFolder
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _customStoriesFolder = new DirectoryInfo(ModuleFolder.FullName + "\\CustomStories");

                return _customStoriesFolder;
            }
            set => _customStoriesFolder = value;
        }

        public DirectoryInfo ModuleFolder
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _moduleFolder = new DirectoryInfo(BasePath.Name + "Modules/LogRaamBannerlordTales");

                return _moduleFolder;
            }
            set => _moduleFolder = value;
        }


        public List<IAct> PlayedActs { get; set; } = new List<IAct>(); // TODO: should import sync data

        public List<IStory> PlayedStories { get; set; } = new List<IStory>(); // TODO: should import sync data

        public List<IStory> Stories { get; set; } = new List<IStory>();

        public DirectoryInfo StoryImagesFolder
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _storyImageFiles = new DirectoryInfo(ModuleFolder.FullName + "\\StoryImages");

                return _storyImageFiles;
            }
            set => _storyImageFiles = value;
        }

        public void AddToPlayedActs(IAct act)
        {
            if (act != null) GameData.Instance.StoryContext.PlayedActs.Add(act);
        }

        public bool AllLinksExistFor(IAct act)
        {
            foreach (var choice in act.Choices)
            {
                if (choice.Triggers == null || choice.Triggers.Count == 0) continue;

                foreach (var trigger in choice.Triggers)
                    if (!TriggerRefExist(trigger))
                        return false;
            }

            return true;
        }

        public bool AllLinksExistFor(ISequence sequence)
        {
            return AllLinksExistFor((IAct)sequence);
        }


        public List<IStory> ImportStoriesFromDisk()
        {
            return ModuleFolder == null
                ? new List<IStory>()
                : new StoryDal().LoadStoriesFromFolder(CustomStoriesFolder);
        }

        #region private

        private bool TriggerActRefExist(ITrigger trigger, IStory S)
        {
            foreach (var act in S.Acts)
                if (trigger.Link == act.Name)
                    return true;

            return false;
        }

        private bool TriggerRefExist(ITrigger trigger)
        {
            foreach (var S in Stories)
            {
                if (TriggerActRefExist(trigger, S)) return true;
                if (TriggerSequenceRefExist(trigger, S)) return true;
            }

            return false;
        }

        private bool TriggerSequenceRefExist(ITrigger trigger, IStory S)
        {
            foreach (var seq in S.Sequences)
                if (trigger.Link == seq.Name)
                    return true;

            return false;
        }

        #endregion
    }
}