// Code written by Gabriel Mailhot, 27/09/2020.

#region

using TalesBase.Stories;
using TalesContract;
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

        public bool AlreadyPlayed()
        {
            foreach (var story in GameData.Instance.StoryContext.PlayedStories)
                if (story.Id == Id)
                    return true;

            return false;
        }

        public bool CanBePlayedOnceAndAlreadyPlayed()
        {
            return Header.CanBePlayedOnlyOnce && AlreadyPlayed();
        }

        public bool IsQualifiedRightNow()
        {
            var rules = new StoryRules(this);
            var audit = new StoryQualificationAudit();

            audit.OneTimeStoryPassed = rules.IsOneTimeStoryNeverPlayed();
            audit.RightTimePassed = rules.IsTheRightTime();
            audit.DependenciesClearancePassed = rules.ItsDependenciesAreCleared();
            audit.StoryTypePassed = rules.IsTheRightStoryType(Header.TypeOfStory);
            audit.RestrictionsPassed = rules.IsNotRestricted();

            return audit.HaveBeenQualified();
        }
    }
}