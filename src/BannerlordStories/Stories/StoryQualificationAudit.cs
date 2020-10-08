// Code written by Gabriel Mailhot, 11/09/2020.

#region

using TalesContract;

#endregion

namespace TalesEntities.Stories
{
    #region

    #endregion

    public class StoryQualificationAudit : IStoryQualificationAudit
    {
        public bool DependenciesClearancePassed { private get; set; }

        public bool OneTimeStoryPassed { private get; set; }

        public bool RestrictionsPassed { private get; set; }

        public bool RightTimePassed { private get; set; }

        public bool StoryTypePassed { private get; set; }

        public bool HaveBeenQualified()
        {
            return OneTimeStoryPassed
                   && RestrictionsPassed
                   && RightTimePassed
                   && DependenciesClearancePassed
                   && StoryTypePassed;
        }
    }
}