// unset

#region

using System;
using System.Collections.Generic;
using System.Linq;
using TalesContract;
using TalesDAL;
using TalesEnums;
using TalesPersistence.Stories;

#endregion

namespace TalesPersistence.Context
{
    public class ActContext
    {
        public Act ChooseQualifiedActFrom(List<Story> stories)
        {
            var acts = new List<Act>();
            foreach (var s in stories)
            {
                if (!s.IsQualifiedRightNow()) continue;

                foreach (var a in s.Acts)
                {
                    var act = new Act(a);

                    if (!act.IsQualifiedRightNow()) continue;

                    acts.Add(act);
                }
            }

            return acts[TalesRandom.GenerateRandomNumber(acts.Count)];
        }

        public List<Story> GetAlreadyPlayedStories()
        {
            var result = new List<Story>();

            foreach (var s in GameData.Instance.StoryContext.Stories)
            {
                var story = new Story(s);

                if (story.CanBePlayedOnceAndAlreadyPlayed()) continue;
                if (!story.AlreadyPlayed()) continue;

                if (story.IsQualifiedRightNow()) result.Add(story);
            }

            return result;
        }

        public List<Story> GetNewStories()
        {
            var result = new List<Story>();

            foreach (var s in GameData.Instance.StoryContext.Stories)
            {
                var story = new Story(s);

                if (story.CanBePlayedOnceAndAlreadyPlayed()) continue;
                if (story.AlreadyPlayed()) continue;

                if (story.IsQualifiedRightNow()) result.Add(story);
            }

            return result;
        }


        public IAct RetrieveActToPlay(StoryType storyType)
        {
            var qualifiedActs = GetAllPrisonerQualifiedActs().Where(n => n.ParentStory.Header.TypeOfStory == storyType).ToList();

            return qualifiedActs.Count > 0
                ? ChooseOneToPlay(qualifiedActs)
                : null;
        }

        #region private

        private IAct ChooseOneToPlay(List<IAct> qualifiedActs)
        {
            if (qualifiedActs.Count == 0) return null;

            var index = new Random().Next(0, qualifiedActs.Count);


            GameData.Instance.StoryContext.AddToPlayedActs(qualifiedActs[index]);

            return qualifiedActs[index];
        }


        private List<IAct> GetAllPrisonerQualifiedActs()
        {
            return GetAllQualifiedActs(GameData.Instance.StoryContext.Stories);
        }


        private List<IAct> GetAllQualifiedActs(List<IStory> stories)
        {
            var result = new List<IAct>();
            foreach (var s in stories)
            {
                if (s.Header.Name.ToUpper() == "TEST") continue;

                var story = new Story(s);

                result.AddRange(GetQualifiedActs(story));
            }

            return result;
        }


        private List<IAct> GetQualifiedActs(IStory story)
        {
            var result = new List<IAct>();
            foreach (var a in story.Acts)
            {
                var act = new Act(a);

                if (act.AlreadyPlayed()) continue;

                if (act.IsQualifiedRightNow()) result.Add(act);
            }

            return result;
        }

        #endregion
    }
}