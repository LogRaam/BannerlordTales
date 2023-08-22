// Code written by Gabriel Mailhot, 02/12/2023.  Updated by  Gabriel Mailhot on 02/20/2023.

#region

using System.Collections.Generic;
using TalesContract;

#endregion

namespace TalesBase.Stories
{
    #region

    #endregion

    public class BaseChoice : IChoice
    {
        public List<IEvaluation> Conditions { get; set; } = new List<IEvaluation>();

        public List<IEvaluation> Consequences { get; set; } = new List<IEvaluation>();

        public string Id => ParentAct.ParentStory.Header.Name.Replace(" ", "") + "_" + ParentAct.Name.Replace(" ", "") + "_" + Text.Replace(" ", "");

        public IAct ParentAct { get; set; }

        public string Text { get; set; }

        public List<ITrigger> Triggers { get; set; } = new List<ITrigger>();

        public bool IsEquivalentTo(IChoice choice)
        {
            for (var i = 0; i < choice.Conditions.Count; i++)
            {
                choice.Conditions[i].IsEquivalentTo(Conditions[i]);
            }

            for (var i = 0; i < choice.Consequences.Count; i++)
            {
                choice.Consequences[i].IsEquivalentTo(Consequences[i]);
            }

            for (var i = 0; i < choice.Triggers.Count; i++)
            {
                choice.Triggers[i].IsEquivalentTo(Triggers[i]);
            }

            //choice.ParentAct.IsEquivalentTo(ParentAct);

            //if (choice.Id != Id) throw new AssertionFailedException("Evaluation expected choice.Id to be equivalent to " + choice.Id + ", but found that its value is " + Id);
            //if (choice.Text != Text) throw new AssertionFailedException("Evaluation expected choice.Text to be equivalent to " + choice.Text + ", but found that its value is " + Text);


            return true;
        }
    }
}