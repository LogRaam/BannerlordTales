// Code written by Gabriel Mailhot, 27/09/2020.

#region

using System;
using TalesContract;
using TalesEntities.Stories;
using TalesEnums;
using TalesPersistence.Context;
using TalesPersistence.Entities;

#endregion

namespace TalesPersistence.Stories
{
    public class Sequence : BaseSequence
    {
        public Sequence(ISequence sequence)
        {
            Choices = sequence.Choices;
            Name = sequence.Name;
            Image = sequence.Image;
            Intro = sequence.Intro;
            Location = sequence.Location;
            Restrictions = sequence.Restrictions;
            ParentStory = sequence.ParentStory;
        }


        public Sequence()
        {
        }

        public bool AllConditionsConformToEvent()
        {
            foreach (var choice in Choices)
            {
                foreach (var condition in choice.Conditions)
                {
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
                }
            }

            return true;
        }


        public bool IsQualifiedRightNow()
        {
            var audit = new ActQualificationAudit
            {
                RightLocationPassed = GameData.Instance.GameContext.IsActLocationValidInContext(this), LinkedSequencesVerified = GameData.Instance.StoryContext.AllLinksExistFor((IAct)this), ConditionsPassed = AllConditionsConformToEvent()
            };


            return audit.HaveBeenQualified();
        }
    }
}