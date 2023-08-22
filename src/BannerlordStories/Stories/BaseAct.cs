// Code written by Gabriel Mailhot, 02/12/2023.  Updated by  Gabriel Mailhot on 02/19/2023.

#region

using FluentAssertions.Execution;
using System.Collections.Generic;
using TalesContract;
using TalesEnums;

#endregion

namespace TalesBase.Stories
{
    #region

    #endregion

    public class BaseAct : IAct
    {
        public List<IChoice> Choices { get; set; } = new List<IChoice>();

        public string Id => ParentStory.Header.Name.Replace(" ", "") + "_" + Name.Replace(" ", "");

        public string Image { get; set; }

        public string Intro { get; set; }

        public Location Location { get; set; } = Location.Unknown;

        public string Name { get; set; }

        public IStory ParentStory { get; set; } = new BaseStory();

        public List<IEvaluation> Restrictions { get; set; } = new List<IEvaluation>();

        public bool IsEquivalentTo(IAct act)
        {
            if (act == null) throw new AssertionFailedException("Evaluation expected act to be evaluated but it is null.");
            if (act.Id != Id) throw new AssertionFailedException("Evaluation expected act.Id to be equivalent to " + act.Id + ", but found that its value is " + Id + ".");
            if (act.Image != Image) throw new AssertionFailedException("Evaluation expected act.Image to be equivalent to " + act.Image + ", but found that its value is " + Image + ".");
            if (act.Name != Name) throw new AssertionFailedException("Evaluation expected act.Name to be equivalent to " + act.Name + ", but found that its value is " + Name + ".");
            if (act.Intro != Intro) throw new AssertionFailedException("Evaluation expected act.Intro to be equivalent to " + act.Intro + ", but found that its value is " + Intro + ".");
            if (act.Location != Location) throw new AssertionFailedException("Evaluation expected act.Location to be equivalent to " + act.Location + ", but found that its value is " + Location + ".");

            for (var i = 0; i < act.Choices.Count; i++)
            {
                act.Choices[i].IsEquivalentTo(Choices[i]);
            }

            for (var i = 0; i < act.Restrictions.Count; i++)
            {
                act.Restrictions[i].IsEquivalentTo(Restrictions[i]);
            }

            //TODO: je dois aussi tester le ParentStory

            return true;
        }
    }
}