// Code written by Gabriel Mailhot, 06/09/2020.

#region

using TalesContract;
using TalesEntities.Stories;

#endregion

namespace BannerlordTales
{
   public class Act : BaseAct
   {
      public Act(IAct act)
      {
         Name = act.Name;
         Intro = act.Intro;
         Location = act.Location;
         Image = act.Image;
         Restrictions = act.Restrictions;
         Id = act.Id;
         Choices = act.Choices;
      }

      public Act()
      {
      }

      public bool IsQualifiedRightNow()
      {
         ActRules rules = new ActRules(this);
         ActQualificationAudit audit = new ActQualificationAudit {
            RightLocationPassed = rules.CorrespondingLocationValidated(),
            LinkedSequencesVerified = rules.LinkedSequencesExists(),
            ConditionsPassed = rules.AllConditionsConformToEvent()
         };

         return audit.HaveBeenQualified();
      }
   }
}