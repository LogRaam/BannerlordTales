// Code written by Gabriel Mailhot, 06/09/2020.

#region

using TalesContract;
using TalesEntities.Stories;

#endregion

namespace BannerlordTales
{
   public class Story : BaseStory
   {
      public Story(IStory baseStory)
      {
         Header = baseStory.Header;
         Acts = baseStory.Acts;
         Id = baseStory.Id;
         Restrictions = baseStory.Restrictions;
      }

      public Story()
      {
      }

      public bool IsQualifiedRightNow()
      {
         StoryRules rules = new StoryRules(this);
         StoryQualificationAudit audit = new StoryQualificationAudit {
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