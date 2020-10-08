// Code written by Gabriel Mailhot, 11/09/2020.

namespace TalesEntities.Stories
{
    public class ActQualificationAudit
    {
        public bool ConditionsPassed { get; set; }
        public bool LinkedSequencesVerified { get; set; }
        public bool RightLocationPassed { get; set; }

        public bool HaveBeenQualified()
        {
            return RightLocationPassed
                   && LinkedSequencesVerified
                   && ConditionsPassed;
        }
    }
}