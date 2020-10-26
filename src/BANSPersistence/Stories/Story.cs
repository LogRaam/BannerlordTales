// Code written by Gabriel Mailhot, 27/09/2020.

#region

using TalesContract;
using TalesEntities.Stories;
using TalesPersistence.Context;

#endregion

namespace TalesPersistence.Stories
{
    public class Story : BaseStory
    {
        public Story(IStory baseStory)
        {
            Header = baseStory.Header;
            Acts = baseStory.Acts;
            Restrictions = baseStory.Restrictions;
            Sequences = baseStory.Sequences;
        }

        public Story()
        {
        }

        public bool CanBePlayedOnceAndAlreadyPlayed()
        {
            if (!Header.CanBePlayedOnlyOnce) return false;

            foreach (var story in GameData.Instance.StoryContext.PlayedStories)
                if (story.Id == Id)
                    return true;

            return false;
        }

        public bool IsQualifiedRightNow()
        {
            var rules = new StoryRules(this);
            var audit = new StoryQualificationAudit
            {
                OneTimeStoryPassed = rules.IsOneTimeStoryNeverPlayed(),
                RightTimePassed = rules.IsTheRightTime(),
                DependenciesClearancePassed = rules.ItsDependenciesAreCleared(),
                StoryTypePassed = rules.IsTheRightStoryType(Header.TypeOfStory),
                RestrictionsPassed = rules.IsNotRestricted()
            };

            return audit.HaveBeenQualified();
        }
    }
}