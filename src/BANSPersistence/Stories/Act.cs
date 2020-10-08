// Code written by Gabriel Mailhot, 27/09/2020.

#region

using System;
using TalesContract;
using TalesEntities.Stories;
using TalesEnums;
using TalesPersistence.Entities;

#endregion

namespace TalesPersistence.Stories
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
            ParentStory = act.ParentStory;
        }

        public Act()
        {
        }

        public bool AlreadyPlayed()
        {
            foreach (var act in GameData.Instance.StoryContext.PlayedActs)
                if (act.ParentStory == ParentStory)
                    if (act.Name == Name)
                        return true;

            return false;
        }


        public bool IsQualifiedRightNow()
        {
            var audit = new ActQualificationAudit
            {
                RightLocationPassed = GameData.Instance.GameContext.IsActLocationValidInContext(this), LinkedSequencesVerified = GameData.Instance.StoryContext.AllLinksExistFor(this), ConditionsPassed = AllConditionsConformToEvent()
            };


            return audit.HaveBeenQualified();
        }

        #region private

        private bool AllConditionsConformToEvent()
        {
            foreach (var choice in Choices)
                foreach (var condition in choice.Conditions)
                    switch (condition.Subject)
                    {
                        case Actor.PLAYER:
                            return new Hero(GameData.Instance.GameContext.Player).IsConsequenceConformFor(condition);

                        case Actor.NPC:
                            return ((Hero)GameData.Instance.GameContext.Captor).IsConsequenceConformFor(condition);

                        case Actor.UNKNOWN:
                            throw new ApplicationException("Act's Consequence evaluation failed: actor unknown.");

                        default:
                            throw new ArgumentOutOfRangeException();
                    }

            return true;
        }

        #endregion
    }
}