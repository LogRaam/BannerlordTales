// Code written by Gabriel Mailhot, 02/12/2023.

#region

using _45_TalesGameState;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TalesContract;
using TalesDAL;
using TaleWorlds.Library;

#endregion

namespace TalesPersistence.Context
{
    #region

    #endregion

    public class StoryContext
    {
        private DirectoryInfo _customStoriesFolder;
        private DirectoryInfo _moduleFolder;
        private DirectoryInfo _storyImageFiles;


        public Textures BackgroundImages { get; set; } = new Textures();

        public DirectoryInfo BodyArmorFolder
        {
            get
            {
                if (CampaignState.CurrentGameStarted()) _moduleFolder = new DirectoryInfo(BasePath.Name + "Modules/SandBoxCore/ModuleData/spitems/body_armors.xml");

                return _moduleFolder;
            }
            set => _moduleFolder = value;
        }

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
            if (act == null) return;

            if (GameData.Instance.StoryContext.PlayedActs.FirstOrDefault(n => n.Id == act.Id) == null) GameData.Instance.StoryContext.PlayedActs.Add(act);
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

        public IAct FindAct(string name)
        {
            foreach (var story in Stories)
                foreach (var act in story.Acts)
                    if (act.Name.ToUpper() == name.ToUpper())
                        return act;

            return null;
        }

        public IAct FindSequence(string name)
        {
            foreach (var story in Stories)
                foreach (var sequence in story.Sequences)
                    if (sequence.Name.ToUpper() == name.ToUpper())
                        return sequence;

            return null;
        }


        public List<IStory> ImportStoriesFromDisk()
        {
            return ModuleFolder == null
                ? new List<IStory>()
                : new StoryDal().LoadStoriesFromFolder(CustomStoriesFolder);
        }

        public bool MenuExist(string stringId)
        {
            foreach (var s in Stories)
            {
                foreach (var act in s.Acts)
                    if (act.Id == stringId)
                        return true;

                foreach (var sequence in s.Sequences)
                    if (sequence.Id == stringId)
                        return true;
            }

            return false;
        }

        #region private

        private bool TriggerActRefExist(ITrigger trigger, IStory s)
        {
            foreach (var act in s.Acts)
                if (trigger.Link == act.Name)
                    return true;

            return false;
        }

        private bool TriggerRefExist(ITrigger trigger)
        {
            foreach (var s in Stories)
            {
                if (TriggerActRefExist(trigger, s)) return true;
                if (TriggerSequenceRefExist(trigger, s)) return true;
            }

            return false;
        }

        private bool TriggerSequenceRefExist(ITrigger trigger, IStory story)
        {
            foreach (var seq in story.Sequences)
                if (trigger.Link == seq.Name)
                    return true;

            return false;
        }

        #endregion
    }
}