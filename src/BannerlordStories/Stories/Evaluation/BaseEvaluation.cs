// Code written by Gabriel Mailhot, 02/12/2023.  Updated by  Gabriel Mailhot on 02/19/2023.

#region

using FluentAssertions.Execution;
using System.Collections.Generic;
using TalesContract;
using TalesEnums;

#endregion

namespace TalesBase.Stories.Evaluation
{
    #region

    #endregion

    public class BaseEvaluation : IEvaluation
    {
        public IEquipments Equipments { get; set; } = new Equipments();
        public INumbers Numbers { get; set; } = new Numbers();
        public IOutcome Outcome { get; set; } = new Outcome();
        public PartyType PartyType { get; set; } = PartyType.NotAssigned;
        public IPersona Persona { get; set; } = new Persona();
        public List<string> Tags { get; set; } = new List<string>();
        public GameTime Time { get; set; } = GameTime.Anytime;

        public bool IsEquivalentTo(IEvaluation evaluation)
        {
            if (evaluation.Time != Time) throw new AssertionFailedException("Evaluation expected Time to be equivalent to " + evaluation.Time + ", but found that its value is " + Time);
            if (evaluation.PartyType != PartyType) throw new AssertionFailedException("Evaluation expected evaluation.PartyType to be equivalent to " + evaluation.PartyType + ", but found that its value is " + PartyType);

            evaluation.Numbers.IsEquivalentTo(Numbers);
            evaluation.Persona.IsEquivalentTo(Persona);
            evaluation.Equipments.IsEquivalentTo(Equipments);
            evaluation.Outcome.IsEquivalentTo(Outcome);


            foreach (var t in evaluation.Tags)
            {
                if (!Tags.Contains(t)) return false;
            }

            return true;
        }
    }
}