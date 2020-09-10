// Code written by Gabriel Mailhot, 25/08/2020.

namespace TalesContract
{
   public interface IStoryQualificationAudit
   {
      public bool DependenciesClearancePassed { set; }

      public bool OneTimeStoryPassed { set; }

      public bool RestrictionsPassed { set; }


      public bool RightTimePassed { set; }

      public bool StoryTypePassed { set; }

      public bool HaveBeenQualified();
   }
}