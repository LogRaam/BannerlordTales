// Code written by Gabriel Mailhot, 02/12/2023.  Updated by  Gabriel Mailhot on 02/19/2023.

#region

using FluentAssertions.Execution;
using TalesContract;

#endregion

namespace TalesBase.Stories.Evaluation
{
    public class Outcome : IOutcome
    {
        public bool Escaping { get; set; }
        public bool PregnancyRisk { get; set; }
        public bool ShouldEquip { get; set; }
        public bool ShouldUndress { get; set; }

        public void IsEquivalentTo(IOutcome outcome)
        {
            if (outcome.Escaping != Escaping) throw new AssertionFailedException("Evaluation expected outcome.Escaping to be equivalent to " + outcome.Escaping + ", but found that its value is " + Escaping);
            if (outcome.PregnancyRisk != PregnancyRisk) throw new AssertionFailedException("Evaluation expected outcome.PregnancyRisk to be equivalent to " + outcome.PregnancyRisk + ", but found that its value is " + PregnancyRisk);
            if (outcome.ShouldEquip != ShouldEquip) throw new AssertionFailedException("Evaluation expected outcome.ShouldEquip to be equivalent to " + outcome.ShouldEquip + ", but found that its value is " + ShouldEquip);
            if (outcome.ShouldUndress != ShouldUndress) throw new AssertionFailedException("Evaluation expected outcome.ShouldUndress to be equivalent to " + outcome.ShouldUndress + ", but found that its value is " + ShouldUndress);
        }
    }
}